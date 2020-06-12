Vue.component('OrganizationModalForm', {
	template: `<Modal :modalProps="modalOrgProps" @newValue="emitValueOrg" @resetValue="resetValue" :refreshInputs="refreshInputs" :inputsKey="modalOrgKey"/>`,
	props: {
		editable: Boolean
	},
	data() {
		return {
			modalOrgKey: 0,
			listIndustry: [],
			workCategories: [],
			regions: [],
			cities: [],
			organizationsListSelect: [],
			geoPositionsSelect: [],
			orgValues: {},
			orgAddress: {},
			editOrgId: null,
			newOrgId: null,
			pageRequestFilters: [],
			modalKey: 0,
			refreshInputs: 0,
			waitAddressButton: false,
			editableOrgModal: false,
			orgAddresses: [],
		};
	},
	computed: {
		selectedRegion() {
			return this.regions[0] ? this.regions[0].value : '';
		},
		orgFormSubmits() {
			const resultArray = [
				{
					name: 'Button',
					props: {
						label: 'Cancel',
						btnType: 'outline-secondary',
						onClick: () => {
							$(`#${this.modalOrgProps.id}`).modal("hide");
						}
					}
				}];
			if (!this.editableOrgModal) {
				resultArray.push(
					{
						name: 'Button',
						props: {
							label: 'Add',
							btnType: 'success',
							btnDOMType: 'submit'
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
							btnDOMType: 'submit'
						}
					}
				);
			}
			return resultArray;
		},
		modalOrglabel() {
			return this.editableOrgModal ? t('org_edit_organization') : t('org_add_organization');
		},
		modalOrgProps() {
			return {
				id: 'organizationModal',
				modalSize: 'lg',
				label: this.modalOrglabel,
				formInputs: [
					{
						name: 'Input',
						props: {
							id: 'company-name',
							type: 'text',
							label: 'Name Organization',
							required: true,
							value: this.orgValues.name,
							className: 'col-12 col-md-6 col-lg-4',
							validator: value => fieldValidationFunc(value, 'varChar128'),
							validatorInput: value => fieldValidationInputFunc(value, 'varChar128')
						}
					},
					{
						name: 'Input',
						props: {
							id: 'company-fiscalCode',
							label: 'Code Fiscal',
							type: 'text',
							required: true,
							value: this.orgValues.fiscalCode,
							className: 'col-12 col-md-6 col-lg-4',
							validator: value => fieldValidationFunc(value, 'fiscalCodeMd'),
							validatorInput: value => fieldValidationInputFunc(value, 'fiscalCodeMd')
						}
					},
					{
						name: 'Select',
						props: {
							id: 'company-geoPosition',
							label: 'Geo Position',
							required: true,
							value: this.orgValues.geoPosition,
							className: 'col-12 col-md-6 col-lg-4',
							options: this.geoPositionsSelect,
						}
					},
					{
						name: 'Select',
						props: {
							id: 'company-industryId',
							label: 'Industry',
							options: this.listIndustry,
							size: 10,
							searchable: true,
							value: this.orgValues.industryId,
							className: 'col-12 col-md-6 col-lg-4'
						}
					},
					{
						name: 'Input',
						props: {
							id: 'company-employees',
							type: 'text',
							label: 'Nr. of Employees',
							value: this.orgValues.employees,
							required: true,
							className: 'col-12 col-md-6 col-lg-4',
							validator: value => fieldValidationFunc(value, 'intNum'),
							validatorInput: value => fieldValidationInputFunc(value, 'intNum')
						}
					},
					{
						name: 'Input',
						props: {
							id: 'company-codTva',
							type: 'text',
							label: 'TVA code',
							value: this.orgValues.codTva,
							className: 'col-12 col-md-6 col-lg-4',
							validator: value => fieldValidationFunc(value, 'varChar50'),
							validatorInput: value => fieldValidationInputFunc(value, 'varChar50')
						}
					},
					{
						name: 'Input',
						props: {
							id: 'company-email',
							type: 'text',
							label: 'Email',
							required: true,
							value: this.orgValues.email,
							className: 'col-12 col-md-6 col-lg-4',
							validator: value => fieldValidationFunc(value, 'email')
						}
					},
					{
						name: 'Input',
						props: {
							id: 'company-phone',
							label: 'Phone',
							type: 'tel',
							required: true,
							inputPrefix: '(+373)',
							value: this.orgValues.phone,
							className: 'col-12 col-md-6 col-lg-4',
							validator: value => fieldValidationFunc(value, 'phone'),
							validatorInput: value => fieldValidationInputFunc(value, 'phone')
						}
					},
					{
						name: 'Input',
						props: {
							id: 'company-website',
							label: 'Web site',
							type: 'text',
							value: this.orgValues.website,
							className: 'col-12 col-md-6 col-lg-4',
							validator: value => fieldValidationFunc(value, 'varChar50'),
							validatorInput: value => fieldValidationInputFunc(value, 'varChar50')
						}
					},
					{
						name: 'hr',
						props: {
							class: 'line-between-inputs'
						}
					},
					{
						name: 'Input',
						props: {
							id: 'company-bank',
							label: 'Bank',
							type: 'text',
							value: this.orgValues.bank,
							className: 'col-12 col-md-6 col-lg-4',
							validator: value => fieldValidationFunc(value, 'varChar500'),
							validatorInput: value => fieldValidationInputFunc(value, 'varChar500')
						}
					},
					{
						name: 'Input',
						props: {
							id: 'company-IBANCode',
							label: 'Code IBAN',
							type: 'text',
							value: this.orgValues.IBANCode,
							className: 'col-12 col-md-6 col-lg-4',
							validator: value => fieldValidationFunc(value, 'varChar128'),
							validatorInput: value => fieldValidationInputFunc(value, 'varChar128')
						}
					},

					{
						name: 'Input',
						props: {
							id: 'company-codSwift',
							type: 'text',
							label: 'Swift code',
							value: this.orgValues.codSwift,
							className: 'col-12 col-md-6 col-lg-4',
							validator: value => fieldValidationFunc(value, 'varChar50'),
							validatorInput: value => fieldValidationInputFunc(value, 'varChar50')
						}
					},
					{
						name: 'hr',
						props: {
							class: 'line-between-inputs'
						}
					},
					{
						name: 'Select',
						props: {
							id: 'company-clientType',
							label: 'Type of Organization',
							value: this.orgValues.clientType,
							required: true,
							options: [
								{
									label: 'Prospect',
									value: 0,
								},
								{
									label: 'Client',
									value: 1
								},
								{
									label: 'Lead',
									value: 2
								},
							],
							className: 'col-12 col-md-6'
						}
					},
					{
						name: 'Select',
						props: {
							id: 'company-isDeleted',
							label: 'Status of Organization',
							required: true,
							disabled: !this.editableOrgModal,
							value: this.orgValues.isDeleted,
							options: [
								{
									label: 'Active',
									value: 'false',
								},
								{
									label: 'Inactive',
									value: 'true'
								}
							],
							className: 'col-12 col-md-6'
						}
					},
					{
						name: 'Select',
						props: {
							id: 'company-listWorkCategory',
							label: 'Work category',
							required: true,
							multiple: true,
							value: this.orgValues.listWorkCategory,
							className: 'col-12 col-md-6',
							options: this.workCategories
						}
					},
					{
						name: 'Input',
						props: {
							id: 'company-afiliat',
							label: 'Afiliat',
							type: 'text',
							value: this.orgValues.afiliat,
							className: 'col-12 col-md-6',
							validator: value => fieldValidationFunc(value, 'zip'),
							validatorInput: value => fieldValidationInputFunc(value, 'zip')
						}
					},
					{
						name: 'hr',
						props: {
							class: 'line-between-inputs'
						}
					},
					{
						name: 'Select',
						props: {
							id: 'addressC-regionId',
							label: 'Region',
							size: 10,
							required: true,
							searchable: true,
							options: this.regions,
							value: this.orgAddress.regionId,
							className: 'col-12 col-md-6 col-lg-3'
						}
					},
					{
						name: 'Select',
						props: {
							id: 'addressC-cityId',
							label: 'City',
							noneSelectedText: 'Select region first',
							options: this.cities,
							required: true,
							size: 10,
							searchable: true,
							value: this.orgAddress.cityId,
							className: 'col-12 col-md-6 col-lg-3'
						}
					},
					{
						name: 'Input',
						props: {
							id: 'addressC-street',
							label: 'Street',
							type: 'text',
							value: this.orgAddress.street,
							className: 'col-12 col-md-6 col-lg-3',
							validator: value => fieldValidationFunc(value, 'varChar128'),
							validatorInput: value => fieldValidationInputFunc(value, 'varChar128')
						}
					},
					{
						name: 'Input',
						props: {
							id: 'addressC-zip',
							label: 'Zip Code',
							type: 'text',
							value: this.orgAddress.zip,
							className: 'col-12 col-md-6 col-lg-3',
							validator: value => fieldValidationFunc(value, 'zip'),
							validatorInput: value => fieldValidationInputFunc(value, 'zip')
						}
					},
					{
						name: 'hr',
						props: {
							class: 'line-between-inputs'
						}
					},
					{
						name: 'Textarea',
						props: {
							id: 'company-description',
							label: 'Description',
							value: this.orgValues.description,
							className: 'col-12'
						}
					}
				],
				formSubmits: this.orgFormSubmits,
				onSubmit: () => {
					if (!this.editableOrgModal) {
						this.addNewOrganization().then(() => {
							this.addOrganizationAddress(this.newOrgId).then(() => {
								this.resetContactValues();
								this.modalContactKey++;
								$(`#${this.modalOrgProps.id}`).modal("hide");
								$(`#${this.modalContactProps.id}`).modal("show");
								this.$emit('action');
							});
						});
					} else {
						this.updateOrganization().then(() => {
							$(`#${this.modalOrgProps.id}`).modal("hide");
							this.$emit('action');
						});
					}
				}
			}
		}
	},
	created: async function () {
		this.editableOrgModal = this.editable;
		this.resetOrgModalValues();
		customAjaxRequest(apiEndpoints.OrganizationHelper.GetSelectorsForOrganization, 'GET').then(result => {
			this.listIndustry = this.convertHelperToSelectList(result.listIndustry);
			this.workCategories = this.convertHelperToSelectList(result.workCategories);
		});
		customAjaxRequest(apiEndpoints.OrganizationAddress.GetAllRegions).then(result => {
			this.regions = this.convertToSelectList(result);
		});
		customAjaxRequest(apiEndpoints.Organization.GetAllOrganization).then(result => {
			this.organizationsListSelect = this.convertToSelectList(result);
		});
		customAjaxRequest(apiEndpoints.OrganizationAddress.GetGeoPosition).then(result => {
			this.geoPositionsSelect = this.convertHelperToSelectList(result);
		});
	},
	methods: {
		async addNewOrganization() {
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.Organization.AddNewOrganization, 'PUT', this.orgValues).then(result => {
					this.newOrgId = result;
					this.tableKey++;
					resolve(true);
				}).catch(e => {
					toast.notifyErrorList(e);
					reject(false);
				});
			});
		},
		async addOrganizationAddress(orgId) {
			this.waitAddressButton = true;
			this.orgAddress.organizationId = orgId;
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.OrganizationAddress.AddOrganizationAddress, 'PUT', this.orgAddress).then(result => {
					this.appendAddressToListing(result).then(() => {
						this.waitAddressButton = false;
					});
					this.resetOrgAddressValues();
					resolve(true);
				}).catch(e => {
					this.waitAddressButton = false;
					toast.notifyErrorList(e);
					reject(false);
				});
			});
		},
		async updateOrganization() {
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.Organization.UpdateOrganization, 'POST', this.orgValues).then(() => {
					resolve(true);
					this.updateOrganizationAddress();
					this.tableKey++;
				}).catch(e => {
					toast.notifyErrorList(e);
					reject(false);
				});
			});
		},
		async loadOrganization(organizationId) {
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.Organization.GetOrganizationById, 'GET', { organizationId }).then(result => {
					resolve(result);
				}).catch(e => {
					toast.notifyErrorList(e);
				});
			});
		},
		async loadRegionCities(regionId) {
			customAjaxRequest(apiEndpoints.OrganizationAddress.GetAllCitiesByRegionId, 'get', { regionId }).then(result => {
				this.cities = this.convertToSelectList(result);
			});
		},
		addAddresses(orgId) {
			this.loadAddresses(orgId).then(() => {
				this.resetOrgAddressValues();
				this.cities = [];
				this.orgAddress.organizationId = orgId;
				this.refreshInputs++;
				this.modalAddressKey++;
				$(`#${this.modalAddressProps.id}`).modal("show");
			});
		},
		updateOrganizationAddress() {
			customAjaxRequest(apiEndpoints.OrganizationAddress.UpdateOrganizationAddress, 'POST', this.orgAddress);
		},
		resetOrgModalValues() {
			this.orgValues = {
				name: null,
				geoPosition: null,
				clientType: 0,
				bank: null,
				email: null,
				phone: null,
				website: null,
				fiscalCode: null,
				IBANCode: null,
				industryId: null,
				employees: null,
				description: null,
				codTva: null,
				codSwift: null,
				listWorkCategory: [],
				isDeleted: false
			}
			this.editOrgId = '';
			this.refreshInputs++;
		},
		editOrganization(orgId) {
			this.loadOrganization(orgId).then(result => {
				this.editableOrgModal = true;
				this.orgValues = result;
				if (result.addresses[0]) {
					this.orgAddress = {
						id: result.addresses[0].id,
						organizationId: result.id,
						regionId: result.addresses[0].city.regionId,
						cityId: result.addresses[0].cityId,
						street: result.addresses[0].street,
						zipCode: result.addresses[0].zip
					}
					this.loadRegionCities(result.addresses[0].city.regionId);
				}
				this.editOrgId = orgId;
				this.refreshInputs++;
				this.modalOrgKey++;
				$(`#${this.modalOrgProps.id}`).modal("show");
			});
		},
		emitValueOrg(val) {
			const newVal = val.value === 'true' ? true : val.value === 'false' ? false : val.value;
			if (val.id.includes('addressC')) {
				this.orgAddress[val.id.replace('addressC-', '')] = newVal;
				if (val.id === 'addressC-regionId') {
					this.loadRegionCities(val.value);
				}
			} else {
				this.orgValues[val.id.replace('company-', '')] = newVal;
			}
		},
		resetValue(val) {
			if (val === 'company-adresses') {
				resetOrgAddressValues();
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
		convertHelperToSelectList(array) {
			return array.map(e => {
				const newObj = {
					label: e.text,
					value: e.value
				}
				return newObj;
			});
		},
		findObjectByPropValue(array, value, prop) {
			return array.find(x => x[prop] === value);
		},
		getRegionName(regionId) {
			return this.findObjectByPropValue(this.regions, regionId, 'value').label;
		},
		openWarningWindow() {
			alert('Organization should have at least one active contact');
		},
	}
});