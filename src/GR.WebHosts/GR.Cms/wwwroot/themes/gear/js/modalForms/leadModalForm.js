const taskDateFormat = 'YYYY/MM/DD';
const taskDatePickerFormat = 'yyyy/mm/dd';
const utils = {
	convertToSelectList: array => {
		return array.map(e => {
			const newObj = {
				label: e.name,
				value: e.id
			}
			return newObj;
		});
	},
	groupBy: (array, key) => {
		return array.reduce(function (result, x) {
			(result[x[key]] = result[x[key]] || []).push(x);
			return result;
		}, {});
	},
	findObjectByPropValue: (array, value, prop) => {
		return array.find(x => x[prop] === value);
	},
	changeObjPropInArrayById: (array, value, prop, id, idProp) => {
		for (var i in array) {
			if (array[i][idProp] == id) {
				array[i][prop] = value;
				break;
			}
		}
	},
	setBodyProgressCursor() {
		$("body").css("cursor", "progress");
	},
	unsetBodyProgressCursor() {
		$("body").css("cursor", "default");
	}
}
Vue.component('LeadModalForm', {
	template: `<Modal :modalProps="modalLeadProps" @newValue="emitValueLead" :refreshInputs="refreshInputs" :inputsKey="modalLeadKey"/>`,
	props: {
		pipelineId: String,
		editable: Boolean
	},
	data() {
		return {
			modalLeadKey: 0,
			leadValues: {
				name: null,
				organizationId: null,
				pipeLineId: this.pipelineId,
				stageId: null,
				leadStateId: null,
				value: null,
				currencyCode: null,
				created: moment().format(taskDateFormat).toString(),
				deadLine: moment().add(5, 'd').format(taskDateFormat).toString(),
				members: [],
				owner: null,
			},
			isAddByStage: false,
			currentStage: '',
			editableLeadModal: false,
			waitAddButton: false,
			continueAddModal: false,
			allAgreements: [],
			sameOrgAgreementId: null,
			waitAddButtonDocument: false,
			documentValues: {},
			documentLeadsSelect: [],
			contractTemplatesSelect: [],
			organisationAddressesSelect: [],
			organisationContactsSelect: [],
			productsSelect: [],
			organizationsListSelect: [],
			pipelinesListSelect: [],
			leadStatesSelect: [],
			currenciesListSelect: [],
			usersListSelect: [],
			workCategoriesSelect: [],
			users: [],
			leads: [],
			refreshInputs: 0,
			usersSelectList: [],
		}
	},
	computed: {
		modalFormSubmits() {
			const resultArray = [
				{
					name: 'Button',
					props: {
						label: 'Cancel',
						btnType: 'outline-secondary',
						waiting: this.waitAddButton,
						onClick: () => {
							$(`#${this.modalLeadProps.id}`).modal("hide");
						}
					}
				}
			];
			if (!this.editableLeadModal) {
				resultArray.push(
					{
						name: 'Button',
						props: {
							label: 'Add',
							btnType: 'success',
							btnDOMType: 'submit',
							waiting: this.waitAddButton,
							onMouseDown: () => {
								this.continueAddModal = false;
							}
						}
					},
					{
						name: 'Button',
						props: {
							label: 'Save & Add Lead',
							waiting: this.waitAddButton,
							btnType: 'success',
							btnDOMType: 'submit',
							onMouseDown: async () => {
								this.continueAddModal = true;
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
							btnDOMType: 'submit',
							onMouseDown: () => {
								this.continueAddModal = false;
							}
						}
					}
				);
			}
			return resultArray;
		},
		modalLeadProps() {
			return {
				id: 'addLead',
				modalSize: '',
				label: this.editableLeadModal ? 'Edit lead' : 'Add lead',
				formInputs: [
					{
						name: 'Input',
						props: {
							id: 'lead-name',
							type: 'text',
							label: 'Name',
							required: true,
							className: 'col-12',
							value: this.leadValues.name,
							validator: value => fieldValidationFunc(value, 'name'),
							validatorInput: value => fieldValidationInputFunc(value, 'name')
						}
					},
					{
						name: 'Datepicker',
						props: {
							id: 'lead-created',
							label: 'Start date',
							disabled: true,
							className: 'col-12 col-md-6',
							format: taskDatePickerFormat,
							value: this.leadValues.created
						}
					},
					{
						name: 'Datepicker',
						props: {
							id: 'lead-deadLine',
							label: 'End date',
							required: true,
							className: 'col-12 col-md-6',
							format: taskDatePickerFormat,
							value: this.leadValues.deadLine
						}
					},
					{
						name: 'Select',
						props: {
							id: 'lead-organizationId',
							label: 'Organization',
							required: true,
							size: 10,
							searchable: true,
							options: this.organizationsListSelect,
							className: 'col-12 col-md-6',
							value: this.leadValues.organizationId
						}
					},
					{
						name: 'Select',
						props: {
							id: 'lead-pipeLineId',
							label: 'PipeLine',
							required: true,
							disabled: true,
							options: this.pipelinesListSelect,
							className: 'col-12 col-md-6',
							value: this.leadValues.pipeLineId
						}
					},
					{
						name: 'Select',
						props: {
							id: 'lead-workCategoryId',
							label: 'Work category',
							required: true,
							size: 10,
							searchable: true,
							options: this.workCategoriesSelect,
							className: 'col-12',
							value: this.leadValues.workCategoryId
						}
					},
					{
						name: 'Select',
						props: {
							id: 'lead-stageId',
							label: 'Stage',
							required: true,
							disabled: this.isAddByStage,
							options: this.pipelineStagesListSelect,
							className: 'col-12 col-md-6',
							value: this.leadValues.stageId
						}
					},
					{
						name: 'Select',
						props: {
							id: 'lead-leadStateId',
							label: 'State',
							required: true,
							options: this.leadStatesSelect,
							className: 'col-12 col-md-6',
							value: this.leadValues.leadStateId
						}
					},
					{
						name: 'Input',
						props: {
							id: 'lead-value',
							label: 'Value',
							type: 'text',
							className: 'col-12 col-md-6',
							value: this.leadValues.value,
							validator: value => fieldValidationFunc(value, 'naturalNum'),
							validatorInput: value => fieldValidationInputFunc(value, 'naturalNum')
						}
					},
					{
						name: 'Select',
						props: {
							id: 'lead-currencyCode',
							label: 'Currency',
							options: this.currenciesListSelect,
							size: 10,
							searchable: true,
							className: 'col-12 col-md-6',
							value: this.leadValues.currencyCode
						}
					},
					{
						name: 'Input',
						props: {
							id: 'lead-unitsNumber',
							label: 'Nr of units',
							type: 'text',
							className: 'col-12 col-md-6',
							value: this.leadValues.unitsNumber,
							validator: value => fieldValidationFunc(value, 'naturalNum'),
							validatorInput: value => fieldValidationInputFunc(value, 'naturalNum')
						}
					},
					{
						name: 'Input',
						props: {
							id: 'lead-commission',
							label: 'Commision',
							type: 'text',
							className: 'col-12 col-md-6',
							value: this.leadValues.commission,
							inputSuffix: '%',
							validator: value => fieldValidationFunc(value, 'naturalNum'),
							validatorInput: value => fieldValidationInputFunc(value, 'naturalNum')
						}
					},
					{
						name: 'Select',
						props: {
							id: 'lead-owner',
							label: 'Owner',
							required: true,
							options: this.usersListSelect,
							value: this.leadValues.owner,
							className: 'col-12'
						}
					},
					{
						name: 'Select',
						props: {
							id: 'lead-members',
							label: 'Members',
							multiple: true,
							searchable: true,
							options: this.membersUsers,
							value: this.leadValues.members,
							className: 'col-12'
						}
					},
				],
				formSubmits: this.modalFormSubmits,
				onSubmit: () => {
					if (!this.editableLeadModal) {
						this.addNewLead().then(() => {
							if (this.continueAddModal) {
								this.resetLeadModalValues();
								this.$emit('action');
							} else {
								$(`#${this.modalLeadProps.id}`).modal("hide");
								this.$emit('action');
							}
						});
					} else {
						this.updateLead().then(() => {
							$(`#${this.modalLeadProps.id}`).modal("hide");
							this.$emit('action');
						});
					}
				}
			}
		}
	},
	created() {
		this.editableLeadModal = this.editable;
		const promises = [
			customAjaxRequest(apiEndpoints.PipeLines.GetPipeLineStages, 'get', { pipeLineId: this.pipelineId }),
			customAjaxRequest(apiEndpoints.Organization.GetAllOrganization),
			customAjaxRequest(apiEndpoints.Users.GetUsers),
			customAjaxRequest(apiEndpoints.Leads.GetAllLeadStates),
			customAjaxRequest(apiEndpoints.PipeLines.GetAll),
			customAjaxRequest(apiEndpoints.CrmCommon.GetAllCurrencies),
			customAjaxRequest(apiEndpoints.WorkCategory.GetAllWorkCategories)
		];
		Promise.all(promises).then(result => {
			this.pipelineStagesListSelect = utils.convertToSelectList(result[0]);
			this.organizationsListSelect = utils.convertToSelectList(result[1]);
			this.usersListSelect = this.convertUsersToSelectList(result[2]);
			this.membersUsers = this.convertUsersToSelectList(result[2]);
			this.usersSelectList = this.convertUsersToSelectList(result[2]);
			this.leadStatesSelect = utils.convertToSelectList(result[3]);
			this.pipelinesListSelect = utils.convertToSelectList(result[4]);
			this.currenciesListSelect = this.convertCurrenciesToSelectList(result[5]);
			this.workCategoriesSelect = this.convertArrayToSelectList(result[6], 'name', 'id');
		});
	},
	methods: {
		async addNewLead() {
			this.waitAddButton = true;
			for (prop in this.leadValues) {
				if (this.leadValues[prop] === null) {
					delete this.leadValues[prop];
				}
			}
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.Leads.AddLead, 'PUT', this.leadValues).then(leadId => {
					if (this.leadValues.owner) {
						this.setLeadOwner(this.leadValues.owner, leadId, this.leadValues.members).then(() => {
							resolve(true);
							this.waitAddButton = false;
						});
					} else {
						this.tableKey++;
						resolve(true);
						this.waitAddButton = false;
					}
				}).catch(e => {
					toast.notifyErrorList(e);
					reject(false);
					this.waitAddButton = false;
				});
			});
		},
		async updateLead() {
			this.waitAddButton = true;
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.Leads.UpdateLead, 'POST', this.leadValues).then(() => {
					if (this.leadValues.owner) {
						this.setLeadOwner(this.leadValues.owner, this.leadValues.id, this.leadValues.members);
					} else {
						this.tableKey++;
					}
					resolve(true);
					this.waitAddButton = false;
				}).catch(e => {
					toast.notifyErrorList(e);
					reject(false);
					this.waitAddButton = false;
				});
			});
		},
		async setLeadOwner(ownerId, leadId, listMembersId = []) {
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.Leads.SetLeadMembers, 'POST', { ownerId, leadId, listMembersId }).then(() => {
					this.tableKey++;
					resolve(true);
				}).catch(e => {
					toast.notifyErrorList(e);
					reject(false);
				});
			});
		},
		async loadLead(leadId) {
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.Leads.GetLeadById, 'GET', { leadId }).then(lead => {
					resolve(lead);
				}).catch(e => {
					toast.notifyErrorList(e);
					reject(false);
				});
			});
		},
		editLead(leadId) {
			utils.setBodyProgressCursor();
			this.editableLeadModal = true;
			this.resetLeadModalValues;
			this.refreshInputs++;
			this.loadLead(leadId).then(value => {
				this.leadValues = value;
				const owners = this.extractTeamIds(this.leadValues.leadMembers, this.defaultOwnerRoleId);
				if (owners) {
					this.leadValues.owner = owners[0];
					utils.changeObjPropInArrayById(this.membersUsers, true, 'disabled', owners[0], 'value');
				}
				this.leadValues.members = this.extractTeamIds(this.leadValues.leadMembers, this.defaultTeamRoleId);
				this.leadValues.created = moment(this.leadValues.created, 'DD.MM.YYYY').format(taskDateFormat);
				this.leadValues.deadLine = moment(this.leadValues.deadLine, 'DD.MM.YYYY').format(taskDateFormat);
				customAjaxRequest(apiEndpoints.Organization.GetOrganizationById, 'GET', { organizationId: value.organizationId }).then(res => {
					this.workCategoriesSelect.forEach(wc => {
						if (res.listWorkCategory.includes(wc.value)) {
							utils.changeObjPropInArrayById(this.workCategoriesSelect, false, 'disabled', wc.value, 'value');
						} else {
							utils.changeObjPropInArrayById(this.workCategoriesSelect, true, 'disabled', wc.value, 'value');
						}
					});
					utils.unsetBodyProgressCursor();
					this.modalLeadKey++;
					$(`#${this.modalLeadProps.id}`).modal("show");
				});
			});
		},
		resetLeadModalValues() {
			this.leadValues = {
				name: null,
				organizationId: null,
				pipeLineId: pipeLineObj.id,
				stageId: null,
				leadStateId: null,
				value: null,
				currencyCode: null,
				created: moment().format(taskDateFormat).toString(),
				deadLine: moment().add(5, 'd').format(taskDateFormat).toString(),
				members: [],
				owner: null,
				unitsNumber: null,
				commission: null,
				workCategoryId: null
			}
			this.modalLeadKey++;
		},
		emitValueLead(val) {
			const newVal = val.value === 'true' ? true : val.value === 'false' ? false : val.value;
			this.leadValues[val.id.replace('lead-', '')] = newVal;
			if (val.id == 'lead-owner') {
				this.membersUsers = this.membersUsers.map(m => {
					m.disabled = false;
					return m;
				});
				utils.changeObjPropInArrayById(this.membersUsers, true, 'disabled', newVal, 'value');
				this.leadValues.members = this.leadValues.members.filter(m => {
					return m != newVal;
				});
			}
			if (val.id == 'lead-organizationId') {
				customAjaxRequest(apiEndpoints.Organization.GetOrganizationById, 'GET', { organizationId: val.value }).then(res => {
					this.leadValues.workCategoryId = null;
					this.workCategoriesSelect.forEach(wc => {
						if (res.listWorkCategory.includes(wc.value)) {
							utils.changeObjPropInArrayById(this.workCategoriesSelect, false, 'disabled', wc.value, 'value');
						} else {
							utils.changeObjPropInArrayById(this.workCategoriesSelect, true, 'disabled', wc.value, 'value');
						}
					});
				});
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
		convertArrayToSelectList(array, textProp, valueProp, translatePrefix = null) {
			return array.map(e => {
				const text = e[textProp];
				const newObj = {
					label: translatePrefix ? window.translate(translatePrefix + text.toLowerCase()) : text,
					disabled: false,
					value: e[valueProp]
				}
				return newObj;
			});
		},
		convertUsersToSelectList(array) {
			return array.map(e => {
				const newObj = {
					label: e.userFirstName ? `${e.userFirstName} ${e.userLastName}` : e.userName,
					value: e.id
				}
				return newObj;
			});
		},
		convertCurrenciesToSelectList(array) {
			return array.map(e => {
				const newObj = {
					label: `${e.name}(${e.symbol})`,
					value: e.code
				}
				return newObj;
			});
		},
		extractTeamOwnerName(team) {
			let owner = utils.findObjectByPropValue(team, this.defaultOwnerRoleId, 'teamRoleId');
			let teamOwnerName = {
				initials: '',
				fullName: '',
			}
			if (owner) {
				teamOwnerName = {
					initials: this.initials(owner.firstName, owner.lastName),
					fullName: `${owner.firstName} ${owner.lastName}`
				}
			}
			return teamOwnerName;
		},
		extractTeam(team, roleId) {
			let returnArray = [];
			team.forEach(m => {
				if (m.teamRoleId == roleId) {
					returnArray.push({
						initials: this.initials(m.firstName, m.lastName),
						fullName: `${m.firstName ? m.firstName : ''} ${m.lastName ? m.lastName : ''}`
					});
				}
			});
			return returnArray;
		},
		extractTeamIds(team, roleId) {
			let returnArray = [];
			if (team) {
				team.forEach(m => {
					if (m.teamRoleId == roleId) {
						returnArray.push(m.userId);
					}
				});
			}
			return returnArray;
		},
		fillLeads(leads) {
			this.leads = leads;
		},
		t(key) {
			return t(key)
		}
	}
});