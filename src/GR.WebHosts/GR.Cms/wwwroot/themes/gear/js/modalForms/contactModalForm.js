Vue.component('ContactModalForm', {
	template: `<Modal :refreshInputs="refreshInputs" :modalProps="modalContactProps" @newValue="emitValueContact" :inputsKey="modalKey"/>`,
	props: {
		editable: Boolean
	},
	data() {
		return {
			organizationsListSelect: [],
			jobPositionsListSelect: [],
			orgId: null,
			searchString: '',
			editContactId: '',
			contactValues: {},
			addAndNew: false,
			waitAddButton: false,
			editableContactModal: false,
			refreshInputs: 0,
			modalKey: 0
		};
	},
	computed: {
		modalContactLabel() {
			return this.editableContactModal ? t('contacts_edit_contact') : t('contacts_add_contact');
		},
		contactFormSubmits() {
			const resultArray = [
				{
					name: 'Button',
					props: {
						label: 'Cancel',
						btnType: 'outline-secondary',
						onClick: () => {
							$(`#${this.modalContactProps.id}`).modal("hide");
						}
					}
				}
			];
			if (!this.editableContactModal) {
				resultArray.push(
					{
						name: 'Button',
						props: {
							label: 'Save',
							btnType: 'success',
							btnDOMType: 'submit',
							waiting: this.waitAddButton,
							onMouseDown: () => {
								this.addAndNew = false;
							}
						}
					},
					{
						name: 'Button',
						props: {
							label: 'Save & Add',
							btnType: 'success',
							btnDOMType: 'submit',
							waiting: this.waitAddButton,
							onMouseDown: () => {
								this.addAndNew = true;
							}
						}
					}
				);
			} else {
				resultArray.push(
					{
						name: 'Button',
						props: {
							label: 'Save',
							btnType: 'success',
							waiting: this.waitAddButton,
							btnDOMType: 'submit'
						}
					}
				);
			}
			return resultArray;
		},
		modalContactProps() {
			return {
				id: 'contactModal',
				label: this.modalContactLabel,
				formInputs: [
					{
						name: 'Input',
						props: {
							id: 'contact-firstName',
							type: 'text',
							label: 'First name',
							required: true,
							className: 'col-12 col-md-6',
							value: this.contactValues.firstName,
							validator: value => fieldValidationFunc(value, 'name'),
							validatorInput: value => fieldValidationInputFunc(value, 'name')
						}
					},
					{
						name: 'Input',
						props: {
							id: 'contact-lastName',
							type: 'text',
							label: 'Last name',
							required: true,
							className: 'col-12 col-md-6',
							value: this.contactValues.lastName,
							validator: value => fieldValidationFunc(value, 'name'),
							validatorInput: value => fieldValidationInputFunc(value, 'name')
						}
					},
					{
						name: 'Select',
						props: {
							id: 'contact-organizationId',
							label: 'Organization',
							required: true,
							searchable: true,
							options: this.organizationsListSelect,
							value: this.contactValues.organizationId,
							className: 'col-12'
						}
					},
					{
						name: 'Input',
						props: {
							id: 'contact-email',
							type: 'email',
							label: 'Email',
							required: true,
							className: 'col-12 col-md-6',
							value: this.contactValues.email,
							validator: value => fieldValidationFunc(value, 'email')
						}
					},
					{
						name: 'Input',
						props: {
							id: 'contact-phone',
							type: 'tel',
							label: 'Phone',
							required: true,
							inputPrefix: '(+373)',
							className: 'col-12 col-md-6',
							value: this.contactValues.phone,
							validator: value => fieldValidationFunc(value, 'phone'),
							validatorInput: value => fieldValidationInputFunc(value, 'phone')
						}
					},
					{
						name: 'Select',
						props: {
							id: 'contact-jobPositionId',
							label: 'Job position',
							options: this.jobPositionsListSelect,
							value: this.contactValues.jobPositionId,
							className: 'col-12'
						}
					},
					{
						name: 'Textarea',
						props: {
							id: 'contact-description',
							label: 'Description',
							className: 'col-12',
							value: this.contactValues.description,
						}
					}
				],
				formSubmits: this.contactFormSubmits,
				onSubmit: () => {
					if (!this.editableContactModal) {
						this.addNewContact().then(() => {
							if (!this.addAndNew) {
								$(`#${this.modalContactProps.id}`).modal("hide");
								this.$emit('action');
							} else {
								this.resetContactModalValues();
								this.refreshInputs++;
								this.$emit('action');
							}
						});
					} else {
						this.updateContact().then(() => {
							$(`#${this.modalContactProps.id}`).modal("hide");
							this.$emit('action');
						});
					}
				}
			}
		}
	},
	created() {
		this.editableContactModal = this.editable;
		this.resetContactModalValues();
		customAjaxRequest(apiEndpoints.Address.GetAllCountries).then(result => {
			this.countries = this.convertToSelectList(result);
		});
		customAjaxRequest(apiEndpoints.Organization.GetAllOrganization).then(result => {
			this.organizationsListSelect = this.convertToSelectList(result);
		});
		customAjaxRequest(apiEndpoints.JobPosition.GetAllJobPositions).then(result => {
			this.jobPositionsListSelect = this.convertToSelectList(result);
		});
	},
	methods: {
		async addNewContact() {
			this.waitAddButton = true;
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.Contact.AddNewContact, 'PUT', this.contactValues).then(() => {
					this.tableKey++;
					resolve(true);
				}).catch(e => {
					toast.notifyErrorList(e);
					reject(false);
				}).finally(() => {
					this.waitAddButton = false;
				});
			});
		},
		async updateContact() {
			this.waitAddButton = true;
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.Contact.UpdateContact, 'POST', this.contactValues).then(() => {
					this.tableKey++;
					resolve(true);
				}).catch(e => {
					toast.notifyErrorList(e);
					reject(false);
				}).finally(() => {
					this.waitAddButton = false;
				});
			});
		},
		async loadContact(contactId) {
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.Contact.GetContactById, 'GET', { contactId }).then(result => {
					resolve(result);
				}).catch(e => {
					toast.notifyErrorList(e);
				});
			});
		},
		editContact(contactId) {
			this.loadContact(contactId).then(result => {
				this.editableContactModal = true;
				this.contactValues = result;
				this.editContactId = contactId;
				this.modalKey++;
				this.refreshInputs++;
				$(`#${this.modalContactProps.id}`).modal("show");
			});
		},
		resetContactModalValues() {
			this.contactValues = {
				organizationId: '',
				email: '',
				phone: '',
				firstName: '',
				lastName: '',
				description: '',
				jobPositionId: ''
			}
			this.editOrgId = '';
		},
		emitValueContact(val) {
			const newVal = val.value === 'true' ? true : val.value === 'false' ? false : val.value;
			this.contactValues[val.id.replace('contact-', '')] = newVal;
			if (val.id == 'contact-organizationId') {
				this.newOrgId == newVal;
			}
		},
		convertToSelectList(array) {
			return array.map(e => {
				const newObj = {
					label: e.name,
					value: e.id
				}
				return newObj;
			});
		}
	}
});