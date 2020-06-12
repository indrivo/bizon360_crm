const taskDateFormat = 'YYYY/MM/DD';
const taskDatePickerFormat = 'yyyy/mm/dd';
Vue.component('TaskModalForm', {
	template: `<Modal :modalProps="modalTaskProps" @newValue="emitValueTask" :refreshInputs="refreshInputs" :inputsKey="modalTaskKey"/>`,
	props: {
		editable: Boolean
	},
	data() {
		return {
			refreshInputs: 0,
			editableTaskModal: false,
			statuses: [],
			priorities: [],
			taskPriorities: [],
			taskStatuses: [],
			waitAddButton: false,
			addAndNew: false,
			usersSelectList: [],
			listLeadsSelect: [],
			taskValues: {},
			modalTaskKey: 0,
		}
	},
	computed: {
		modalTaskSubmits() {
			const resultArray = [
				{
					name: 'Button',
					props: {
						label: 'Cancel',
						btnType: 'outline-secondary',
						onClick: () => {
							$(`#${this.modalTaskProps.id}`).modal("hide");
						}
					}
				}];
			if (!this.editableTaskModal) {
				resultArray.push(
					{
						name: 'Button',
						props: {
							label: 'Add',
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
							label: 'Add & new',
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
							btnDOMType: 'submit',
							waiting: this.waitAddButton,
							onMouseDown: () => {
								this.addAndNew = false;
							}
						}
					}
				);
			}
			return resultArray;
		},
		modalTasklabel() {
			return this.editableTaskModal ? 'Edit task' : 'Add task';
		},
		modalTaskProps() {
			return {
				id: 'taskModal',
				modalSize: '',
				label: this.modalTasklabel,
				formInputs: [
					{
						name: 'Input',
						props: {
							id: 'task-name',
							type: 'text',
							label: 'Name',
							required: true,
							value: this.taskValues.name,
							className: 'col-12',
							validator: value => fieldValidationFunc(value, 'name'),
							validatorInput: value => fieldValidationInputFunc(value, 'name')
						}
					},
					{
						name: 'Select',
						props: {
							id: 'task-leadId',
							label: 'Lead',
							options: this.listLeadsSelect,
							value: this.taskValues.leadId,
							size: 10,
							searchable: true,
							className: 'col-12'
						}
					},
					{
						name: 'Datepicker',
						props: {
							id: 'task-startDate',
							label: 'Start date',
							required: true,
							format: taskDatePickerFormat,
							value: this.taskValues.startDate,
							className: 'col-12 col-md-6'
						}
					},
					{
						name: 'Datepicker',
						props: {
							id: 'task-endDate',
							label: 'End date',
							required: true,
							format: taskDatePickerFormat,
							value: this.taskValues.endDate,
							className: 'col-12 col-md-6'
						}
					},
					{
						name: 'Select',
						props: {
							id: 'task-status',
							label: 'Status',
							options: this.taskStatuses,
							value: this.taskValues.status,
							className: 'col-12 col-md-6'
						}
					},
					{
						name: 'Select',
						props: {
							id: 'task-taskPriority',
							label: 'Priority',
							options: this.taskPriorities,
							value: this.taskValues.taskPriority,
							className: 'col-12 col-md-6'
						}
					},
					{
						name: 'Select',
						props: {
							id: 'task-userTeam',
							label: 'Assigne',
							searchable: true,
							multiple: true,
							options: this.usersSelectList,
							value: this.taskValues.userTeam,
							className: 'col-12'
						}
					},
					{
						name: 'Textarea',
						props: {
							id: 'task-description',
							label: 'Description',
							required: true,
							value: this.taskValues.description,
							className: 'col-12'
						}
					}
				],
				formSubmits: this.modalTaskSubmits,
				onSubmit: () => {
					if (!this.editableTaskModal) {
						if (this.addAndNew) {
							this.addNewTask().then(() => {
								this.resetTaskValues();
								this.refreshInputs++;
								this.$emit('action');
							});
						} else {
							this.addNewTask().then(() => {
								$(`#${this.modalTaskProps.id}`).modal("hide");
								this.$emit('action');
							});
						}
					} else {
						this.updateTask().then(() => {
							$(`#${this.modalTaskProps.id}`).modal("hide");
							this.$emit('action');
						});
					}
				}
			}
		},
	},
	created() {
		this.resetTaskValues();
		this.refreshInputs++;
		const promises = [
			customAjaxRequest(apiEndpoints.TaskManager.GetTaskPriorityList),
			customAjaxRequest(apiEndpoints.TaskManager.GetTaskStatusList),
			customAjaxRequest(apiEndpoints.Users.GetUsers),
			customAjaxRequest(apiEndpoints.Leads.GetAllLeads)
		];
		Promise.all(promises).then(result => {
			this.taskPriorities = this.convertArrayToSelectList(result[0], 'text', 'value', 'system_taskmanager_');
			this.priorities = result[0];
			this.taskStatuses = this.convertArrayToSelectList(result[1], 'text', 'value', 'system_taskmanager_');
			this.statuses = result[1];
			this.usersSelectList = this.convertUsersToSelectList(result[2]);
			this.listLeadsSelect = this.convertArrayToSelectList(result[3], 'name', 'id');
		});
	},
	methods: {
		async addNewTask() {
			this.waitAddButton = true;
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.TaskManager.CreateTask, 'POST', this.taskValues).then(() => {
					this.waitAddButton = false;
					this.tableKey++;
					resolve(true);
				}).catch(e => {
					this.waitAddButton = false;
					toast.notifyErrorList(e);
					reject(false);
				});
			});
		},
		async updateTask() {
			this.waitAddButton = true;
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.TaskManager.UpdateTask, 'POST', this.taskValues).then(() => {
					this.waitAddButton = false;
					this.tableKey++;
					resolve(true);
				}).catch(e => {
					this.waitAddButton = false;
					toast.notifyErrorList(e);
					reject(false);
				});
			});
		},
		async loadTask(id) {
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.TaskManager.GetTask, 'GET', { id }).then(result => {
					resolve(result);
				}).catch(e => {
					toast.notifyErrorList(e);
					reject(e);
				});
			});
		},
		getPriorityBadge(val) {
			let badgeClass = 'badge-outline-default';
			switch (val.value) {
				case '0':
					badgeClass = 'badge-outline-info';
					break;
				case '1':
					badgeClass = 'badge-outline-primary';
					break;
				case '2':
					badgeClass = 'badge-outline-warning';
					break;
				case '3':
					badgeClass = 'badge-outline-danger';
					break;
			}
			return `<span class="badge ${badgeClass}">${val.label}</span>`;
		},
		editTask(taskId) {
			this.waitAddButton = false;
			this.editableTaskModal = true;
			this.loadTask(taskId).then(result => {
				this.taskValues = {
					id: result.id,
					name: result.name,
					description: result.description,
					startDate: moment(result.startDate, 'DD.MM.YYYY').format(taskDateFormat),
					endDate: moment(result.endDate, 'DD.MM.YYYY').format(taskDateFormat),
					userTeam: result.userTeam,
					leadId: result.leadId,
					taskPriority: result.taskPriority,
					status: result.status
				};
				this.modalTaskKey++;
				this.refreshInputs++;
				$(`#${this.modalTaskProps.id}`).modal("show");
			});
		},
		resetTaskValues() {
			this.taskValues = {
				name: '',
				description: '',
				startDate: moment().format(taskDateFormat).toString(),
				endDate: moment().add(1, 'h').format(taskDateFormat).toString(),
				leadId: null,
				userTeam: [],
				taskPriority: '0',
				status: '0'
			}
		},
		emitValueTask(val) {
			const newVal = val.value === 'true' ? true : val.value === 'false' ? false : val.value;
			this.taskValues[val.id.replace('task-', '')] = newVal;
		},
		convertArrayToSelectList(array, textProp, valueProp, translatePrefix = null) {
			return array.map(e => {
				const text = e[textProp];
				const newObj = {
					label: translatePrefix ? window.translate(translatePrefix + text.toLowerCase()) : text,
					value: e[valueProp]
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
		}
	}
});