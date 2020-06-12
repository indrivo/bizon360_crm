Vue.component('AgreementModalForm', {
	template: `
	<div>
		<Modal :refreshInputs="refreshInputs" :modalProps="modalProps" @newValue="emitModalValue" :inputsKey="modalKey"/>
		<Modal :refreshInputs="refreshInputs" :modalProps="modalsameOrgAgreements"/>
	</div>
	`,
	props: {
		editable: Boolean
	},
	data() {
		return {
			modalKey: 0,
			contractTemplatesSelect: [],
			leadsSelect: [],
			organisationsSelect: [],
			organisationAddressesSelect: [],
			organisationContactsSelect: [],
			usersSelect: [],
			productsSelect: [],
			workCategoriesSelect: [],
			modalValues: {},
			isOrgSelectDisabled: false,
			waitAddButton: false,
			editableModal: false,
			refreshInputs: 0
		};
	},
	computed: {
		modalLabel() {
			return this.editableModal ? 'Edit agreement' : 'Add agreement';
		},
		modalProps() {
			return {
				id: 'agreementModal',
				label: this.modalLabel,
				formInputs: [
					{
						name: 'Input',
						props: {
							id: 'agreement-name',
							type: 'text',
							label: 'Name',
							required: true,
							className: 'col-12',
							value: this.modalValues.name,
							validator: value => fieldValidationFunc(value, 'varChar128'),
							validatorInput: value => fieldValidationInputFunc(value, 'varChar128')
						}
					},
					{
						name: 'Select',
						props: {
							id: 'agreement-contractTemplateId',
							label: 'Template Contract',
							required: true,
							options: this.contractTemplatesSelect,
							className: 'col-12',
							value: this.modalValues.contractTemplateId
						}
					},
					{
						name: 'Select',
						props: {
							id: 'agreement-organizationId',
							label: 'Organization',
							required: true,
							options: this.organisationsSelect,
							size: 10,
							className: 'col-12',
							value: this.modalValues.organizationId
						}
					},
					{
						name: 'Select',
						props: {
							id: 'agreement-leadId',
							label: 'Lead',
							required: true,
							size: 10,
							searchable: true,
							options: this.leadsSelect,
							className: 'col-12',
							value: this.modalValues.leadId,
							noneSelectedText: this.modalValues.organizationId && this.leadsSelect.length == 0 ? 'Please add at least one lead to this organization' : 'Nothing selected'
						}
					},
					{
						name: 'Select',
						props: {
							id: 'agreement-organizationAddressId',
							label: 'Organization address',
							required: true,
							options: this.organisationAddressesSelect,
							size: 10,
							searchable: true,
							className: 'col-12',
							value: this.modalValues.organizationAddressId,
							noneSelectedText: this.modalValues.organizationId && this.organisationAddressesSelect.length == 0 ? 'Please add at least one address to this organization' : 'Nothing selected'
						}
					},
					{
						name: 'Select',
						props: {
							id: 'agreement-contactId',
							label: 'Contact',
							required: true,
							size: 10,
							searchable: true,
							options: this.organisationContactsSelect,
							className: 'col-12 col-md-6',
							value: this.modalValues.contactId,
							noneSelectedText: this.modalValues.organizationId && this.organisationContactsSelect.length == 0 ? 'Please add at least one contact to this organization' : 'Nothing selected'
						}
					},
					{
						name: 'Input',
						props: {
							id: 'agreement-commission',
							label: 'Commission',
							type: 'text',
							required: true,
							className: 'col-12 col-md-6',
							inputSuffix: '%',
							value: this.modalValues.commission,
							validator: value => fieldValidationFunc(value, 'percentage'),
							validatorInput: value => fieldValidationInputFunc(value, 'percentage')
						}
					},
					{
						name: 'Select',
						props: {
							id: 'agreement-userId',
							label: 'Responsible',
							required: true,
							size: 10,
							searchable: true,
							options: this.usersSelect,
							className: 'col-12',
							value: this.modalValues.userId
						}
					},
					{
						name: 'Textarea',
						props: {
							id: 'agreement-description',
							label: 'Description',
							required: true,
							value: this.modalValues.description,
							className: 'col-12'
						}
					}
				],
				formSubmits: [
					{
						name: 'Button',
						props: {
							label: 'Cancel',
							btnType: 'outline-secondary',
							onClick: () => {
								$(`#${this.modalProps.id}`).modal("hide");
							}
						}
					},
					{
						name: 'Button',
						props: {
							label: 'Save',
							btnType: 'success',
							waiting: this.waitAddButton,
							btnDOMType: 'submit'
						}
					}
				],
				onSubmit: () => {
					if (!this.editableModal) {
						this.checkForDuplicateAndAdd().then(id => {
							this.modalValues.id = id;
							$(`#${this.modalProps.id}`).modal("hide");
							$(`#${this.modalsameOrgAgreements.id}`).modal("show");
							this.$emit('action');
						}).catch(() => {
							this.addNewEntity().then(() => {
								$(`#${this.modalProps.id}`).modal("hide");
							});
							this.$emit('action');
						});
					} else {
						this.updateEntity().then(() => {
							$(`#${this.modalProps.id}`).modal("hide");
							this.$emit('action');
						});
					}
				}
			}
		},
		modalsameOrgAgreements() {
			return {
				id: 'orgAgreementsModal',
				label: 'Attention!',
				formInputs: [
					{
						name: 'TextBlock',
						props: {
							value: `Contract for this Organization just exist. Do you want to overwrite it?`
						}
					}
				],
				formSubmits: [
					{
						name: 'Button',
						props: {
							label: 'Yes',
							btnType: 'warning',
							onClick: () => {
								this.modalValues.id = this.sameOrgAgreementId;
								this.updateEntity().then(() => {
									$(`#${this.modalsameOrgAgreements.id}`).modal("hide");
								});
							}
						}
					},
					{
						name: 'Button',
						props: {
							label: 'Cancel',
							btnType: 'outline-secondary',
							onClick: () => {
								this.modalKey++;
								$(`#${this.modalsameOrgAgreements.id}`).modal("hide");
								$(`#${this.modalProps.id}`).modal("show");
							}
						}
					},
					{
						name: 'Button',
						props: {
							label: 'Save as new',
							btnType: 'success',
							onClick: () => {
								this.addNewEntity().then(() => {
									$(`#${this.modalsameOrgAgreements.id}`).modal("hide");
								});
							}
						}
					},
				]
			}
		},
	},
	created() {
		this.editableModal = this.editable;
		this.resetModalValues();
		const promises = [
			customAjaxRequest(apiEndpoints.Contract.GetAllContractTemplate),
			customAjaxRequest(apiEndpoints.Organization.GetAllOrganization),
			customAjaxRequest(apiEndpoints.Users.GetUsers)
		];
		Promise.all(promises).then(result => {
			this.contractTemplatesSelect = this.convertToSelectList(result[0]);
			this.organisationsSelect = this.convertToSelectList(result[1]);
			this.usersSelect = this.convertUsersToSelectList(result[2]);
		});
	},
	methods: {
		async checkForDuplicateAndAdd() {
			const orgId = this.modalValues.organizationId;
			let sameOrgAgreements = this.allAgreements.filter(a => a.organizationId === orgId);
			sameOrgAgreements = sameOrgAgreements.sort((x, y) => {
				return moment(x.created, 'DD.MM.YYYY').isAfter(y.creted, 'DD.MM.YYYY') ? -1 : 1;
			});
			return new Promise((resolve, reject) => {
				if (sameOrgAgreements.length > 0) {
					this.sameOrgAgreementId = sameOrgAgreements[0].id;
					resolve(true);
				} else {
					reject(false);
				}
			});
		},
		async addNewEntity() {
			this.waitAddButton = true;
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.Agreement.AddAgreement, 'PUT', this.modalValues).then(() => {
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
		async updateEntity() {
			this.waitAddButton = true;
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.Agreement.UpdateAgreement, 'POST', this.modalValues).then(() => {
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
		async loadEntity(entityId) {
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.Agreement.GetAgreementById, 'GET', { agreementId: entityId }).then(result => {
					resolve(result);
				}).catch(e => {
					toast.notifyErrorList(e);
				});
			});
		},
		loadOrganizationAdditionalData(organizationId) {
			const promises = [
				customAjaxRequest(apiEndpoints.Leads.GetLeadsByOrganizationId, 'GET', { organizationId }),
				customAjaxRequest(apiEndpoints.Contact.GetContactByOrganizationId, 'GET', { organizationId }),
				customAjaxRequest(apiEndpoints.OrganizationAddress.GetAddressesByOrganizationId, 'GET', { organizationId })
			];
			Promise.all(promises).then(result => {
				this.leadsSelect = this.convertToSelectList(result[0]);
				this.organisationContactsSelect = this.convertContactsToSelectList(result[1]);
				this.organisationAddressesSelect = this.convertAdddressToSelectList(result[2]);
				this.modalKey++;
			});
		},
		editEntity(entityId) {
			this.loadEntity(entityId).then(result => {
				this.editableModal = true;
				this.modalValues = {
					id: entityId,
					name: result.name,
					leadId: result.leadId,
					organizationId: result.organizationId,
					contactId: result.contactId,
					organizationAddressId: result.organizationAddressId,
					userId: result.userId,
					contractTemplateId: result.contractTemplateId,
					commission: result.commission,
					productId: result.productId,
					description: result.description
				}
				this.updateEntityId = entityId;
				if (result.organizationId) {
					this.loadOrganizationAdditionalData(result.organizationId);
				}
				this.modalKey++;
				this.refreshInputs++;
				$(`#${this.modalProps.id}`).modal("show");
			});
		},
		generateContract(agreementId) {
			window.open(`${window.location.origin}${apiEndpoints.Agreement.GenerateFileContractForAgreement}?agreementId=${agreementId}`, '_blank');
		},
		resetModalValues() {
			this.modalValues = {
				name: null,
				leadId: null,
				organizationId: null,
				contactId: null,
				organizationAddressId: null,
				userId: null,
				contractTemplateId: null,
				commission: null
			}
		},
		emitModalValue(val) {
			const newVal = val.value === 'true' ? true : val.value === 'false' ? false : val.value;
			this.modalValues[val.id.replace('agreement-', '')] = newVal;
			if (val.id == 'agreement-organizationId') {
				this.loadOrganizationAdditionalData(val.value);
				this.modalKey++;
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
		},
		convertContactsToSelectList(array) {
			return array.map(e => {
				const newObj = {
					label: `${e.firstName} ${e.lastName}`,
					value: e.id
				}
				return newObj;
			});
		},
		convertUsersToSelectList(array) {
			return array.map(e => {
				return {
					label: e.userFirstName ? `${e.userFirstName} ${e.userLastName}` : e.userName,
					value: e.id
				}
			});
		},
		convertAdddressToSelectList(array) {
			return array.map(e => {
				const newObj = {
					label: `r.${e.city.region.name}, ${e.city.name} ${e.street ? ', ' + e.street : ''}`,
					value: e.id
				}
				return newObj;
			});
		},
		getUserName(userId) {
			const user = this.findObjectByPropValue(this.usersSelect, userId, 'value');
			return user.label;
		},
		findObjectByPropValue: (array, value, prop) => {
			return array.find(x => x[prop] === value);
		}
	}
});