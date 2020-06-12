Vue.use(Vuetable);
Vue.component('Table', {
	template: `
		<div :id="tableId" class="position-relative">
			<div v-show="!dataLoaded" class="table-loader" :style="'height:' + loaderHeight"><Loader/></div>
			<vuetable
				ref="vuetable"
				:key="reload"
				:api-url="apiUrl"
				:fields="fieldsWithCheckbox"
				:css="css"
				:row-class="rowClass"
				v-on:vuetable:row-clicked="rowClick"
				v-on:vuetable:loading="loadingStart"
				v-on:vuetable:loaded="loadingStop"
				v-on:vuetable:load-success="loadSuccess"
				v-on:vuetable:pagination-data="onPaginationData"
				:sort-order="sortOrder ? sortOrder : []"
				:query-params="makeQueryParams"
				:http-method="httpMethod"
				:http-options="httpOptions"
				:pagination-path="paginationPath"
				:data-path="dataPath"
				:per-page="initialPageSize"
				>
				<slot v-for="(_, name) in $slots" :name="name" :slot="name" />
				<template v-for="(_, name) in $scopedSlots" :slot="name" slot-scope="slotData">
					<div :class="'slot-' + name">
						<slot :name="name" v-bind="slotData" />
					</div>
				</template>
				<div slot="id" slot-scope="props" class="custom-control custom-checkbox">
					<input type="checkbox" class="row-id custom-control-input" :id="props.rowData.id" v-model="checkedRows" :value="props.rowData.id" v-on:click.stop.prevent/>
					<label class="custom-control-label" :for="props.rowData.id"></label>
				</div>
				<slot slot="isDeleted" slot-scope="props">
					<span v-if="props.rowData.isDeleted" class="badge badge-outline-secondary">Inactive</span>
					<span v-else class="badge badge-outline-success">Active</span>
				</slot>
				<div slot="drag-drop-rows" slot-scope="props" v-if="!props.rowData.isDeleted" class="drag-icon">
					<span :id="props.rowData.id" :data-order="props.rowData.order" class="drag-icon-element" @click.stop @mousedown="mosedownDrag" @mouseup="mouseUpDrag" @mouseleave="mouseLeaveDrag"></span>
				</div>
			</vuetable>
			<div v-show="totalEntriesCount > 0">
				<div class="d-flex mt-20px ml-20px mr-20px mb-20px">
					<div v-if="dataPath == 'result.result'"> {{firstRowOnPage | numberWithCommas}}-{{lastRowOnPage | numberWithCommas}} of {{ totalEntriesCount | numberWithCommas}}</div>
					<div class="ml-auto">
						<span class="mr-1">Show</span>
						<select class="per-page-table-select" v-model="showPerPage">
							<option value="10">10</option>
							<option value="20">20</option>
							<option value="50">50</option>
							<option value="100">100</option>
						</select>
						<span class="ml-1">items per page</span>
					</div>
					<div class="d-flex ml-5">
						<div v-show="dataPath == 'result.result' && totalEntriesCount > currentPageSize">
							<vuetable-pagination
								:css="paginationCSS"
								v-on:vuetable-pagination:change-page="onChangePage"
								ref="pagination">
							</vuetable-pagination>
						</div>
					</div>
				</div>
			</div>
		</div>
	`,
	data() {
		return {
			checkedRows: [],
			selectedAll: false,
			isVisibilityFieldsDropdownOpen: false,
			currentPage: 1,
			dataLoaded: false,
			loaderHeight: '150px',
			tableHeight: '150px',
			loadMoreCount: null,
			paginationCSS: {
				wrapperClass: 'd-flex table-pagination',
				activeClass: 'active',
				disabledClass: 'disabled',
				pageClass: 'page-item',
				linkClass: '',
				paginationClass: 'pagination',
				paginationInfoClass: 'float-left',
				dropdownClass: 'form-control',
				icons: {
					prev: 'fa fa-chevron-left',
					next: 'fa fa-chevron-right',
				}
			},
			currentPageSize: 0,
			totalEntriesCount: 0,
			firstRowOnPage: 0,
			lastRowOnPage: 0,
			showPerPage: "20"
		}
	},
	props: {
		//fields that must be displayed in table, must have visible property to work properly 
		fields: Array,
		//table ID
		tableId: String,
		//sortOrder for vuetable component, see vuetable documentation 
		sortOrder: Array,
		//apiUrl for vuetable component, see vuetable documentation  
		apiUrl: String,
		//http method (get, post, put, delete, ...)
		httpMethod: String,
		//additional options for table data reqest(sort, search)
		httpOptions: Object,
		//initial row count after table load
		initialPageSize: Number,
		// increment this number to reload page
		reload: Number,
		// if table request has custom query parameters 
		hasCustomQUeryparams: Boolean,
		// custom query parameters will be added if hasCustomQUeryparams == true
		customQueryparams: Object,
		// after request done table will navigate into response to find array with data
		dataPath: String,
		// if true will disable show/hide column feature
		withoutHideColumn: Boolean,
		// if true will enable drag and drop rows feature 
		draggableRows: Boolean,
		// if drag and drop enabled optionally can add options for Jquery .sortable()
		draggableOptions: {
			type: Object,
			default: () => {
				return {
					axis: 'y',
					items: 'tr'
				}
			}
		},
		//haveCOntextMenu
		haveContextMenu: Boolean,
		//right click on single row actions
		actionsSingle: {
			type: Object,
			default: () => {
				return {

				}
			},
		},
		//right click on single disabled row actions
		actionsSingleDisabled: {
			type: Object,
			default: () => {
				return {}
			}
		},
		//right click on single isSystem row actions
		actionsSingleSystem: {
			type: Object,
			default: () => {
				return {}
			}
		},
		//right click on multiple rows actions
		actionsMultiple: {
			type: Object,
			default: () => {
				return {}
			}
		},
		//right click on multiple rows actions
		actionsMultipleDisabled: {
			type: Object,
			default: () => {
				return {}
			}
		},
		//additional class for table element
		className: String,
	},
	computed: {
		showPerPageInt() {
			return parseInt(this.showPerPage);
		},
		paginationPath() {
			return this.dataPath.replace('result.', '');
		},
		fieldsWithCheckbox() {
			let columnsMarkup = '';
			this.fields.forEach(field => {
				columnsMarkup = columnsMarkup + `
					<div class="custom-control custom-checkbox">
						<input type="checkbox" class="custom-control-input field-visibility" id="${field.name}-visibility" ${field.visible ? 'checked' : ''}/>
						<label class="custom-control-label" for="${field.name}-visibility">${field.title}</label>
					</div>
				`;
			});

			const newArray = [...this.fields];

			if (this.draggableRows) {
				newArray.unshift({
					name: 'drag-drop-rows',
					title: ``,
					dataClass: 'drag-cell'
				});
			}

			newArray.unshift({
				name: 'id',
				title: `
					<div class="custom-control custom-checkbox">
						<input type="checkbox" class="custom-control-input" id="${this.tableId}-selectAllCheckbox"/>
						<label class="custom-control-label" for="${this.tableId}-selectAllCheckbox"></label>
					</div>`,
			});

			if (!this.withoutHideColumn) {
				newArray.push({
					name: 'show-hide-fields',
					title: `
						<div class="dropdown datatables-dropdown">
							<div class="more-vertical" data-toggle="dropdown"></div>
							<div class="px-3 dropdown-menu dropdown-menu-right" x-placement="bottom-end">
								${columnsMarkup}
							</div>
						</div>`
				});
			}

			return newArray;
		}, //add checkbox to rows and display them in table
		loadMoreLabel() {
			return `Load more <span class="count-rectangle ml-1">${this.loadMoreCount}</span>`;
		},// label for "Load more" button
		css() {
			return {
				tableWrapper: '',
				tableHeaderClass: 'fixed',
				tableBodyClass: 'fixed',
				tableClass: `table bizon-datatable ${this.className}`,
				loadingClass: 'loading',
				ascendingIcon: 'blue chevron up icon',
				descendingIcon: 'blue chevron down icon',
				ascendingClass: 'sorted-asc',
				descendingClass: 'sorted-desc',
				sortableIcon: 'grey sort icon',
				handleIcon: 'grey sidebar icon'
			}
		}
	},
	mounted() {
		this.addJqueryEventListeners();
	},
	updated() {
		this.addJqueryEventListeners();
		if (this.isVisibilityFieldsDropdownOpen) {
			$(`#${this.tableId} .datatables-dropdown .more-vertical`).dropdown('toggle');
		}
	},
	beforeUpdate() {
		this.removeJqueryEventListeners();
	},
	beforeDestroy() {
		this.removeJqueryEventListeners();
		$(`#${this.tableId} tbody`).sortable('destroy');
	},
	filters: {
		numberWithCommas: x => {
			return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
		}
	},
	methods: {
		onPaginationData(paginationData) {
			const last_page = Math.ceil(paginationData.rowCount / paginationData.showPerPageInt);
			const newPaginationData = {
				total: paginationData.rowCount,
				per_page: paginationData.showPerPageInt,
				current_page: paginationData.currentPage,
				last_page,
				prev_page_url: null,
				next_page_url: null,
				from: paginationData.firstRowOnPage,
				to: paginationData.lastRowOnPage
			}
			this.currentPageSize = paginationData.lastRowOnPage - paginationData.firstRowOnPage + 1;
			this.totalEntriesCount = paginationData.rowCount;
			this.firstRowOnPage = paginationData.firstRowOnPage;
			this.lastRowOnPage = paginationData.lastRowOnPage;
			this.$refs["pagination"].setPaginationData(newPaginationData);
		},
		onChangePage(page) {
			if (page == this.currentPage) {
				return false;
			} else if (page == 'next') {
				if (this.currentPageSize >= 20 && this.totalEntriesCount > 20) {
					this.currentPage++;
				} else {
					return false;
				}
			} else if (page == 'prev') {
				if (this.currentPage > 0 && this.currentPage != 1) {
					this.currentPage--;
				} else {
					return false;
				}
			} else {
				this.currentPage = page;
			}
			this.$refs["vuetable"].reload();
		},
		emitAction(key, value, atLeastOneActive) {
			this.$emit('triggeredMenuAction', { key, value, atLeastOneActive });
		},
		rowClass(data) {
			let className = '';
			if (data.isDeleted) {
				className += ' disabled-row'
			}
			if (data.isSystem || data.isNoEditable) {
				className += ' system-row'
			}
			if (this.checkedRows.includes(data.id)) {
				className += ' selected'
			}
			return className;
		},
		changeChecboxVal(val) {
			$(`#${this.tableId}-selectAllCheckbox`).prop("checked", val);
		},
		rowClick(data) {
			if (this.checkedRows.includes(data.data.id)) {
				this.checkedRows = this.checkedRows.filter(e => e !== data.data.id);
			} else {
				this.checkedRows.push(data.data.id);
			}
		},
		markAll() {
			if (allMarked) {
				this.checkedRows = [];
			} else {
				this.$refs["vuetable"].tableData.map(e => {
					this.checkedRows.push(e.id);
				});
			}
		},
		addJqueryEventListeners() {
			if (this.haveContextMenu) {
				$.contextMenu({
					selector: `#${this.tableId} .vuetable-body tr`,
					build: (_, e) => {
						if ($(e.target).closest('.vuetable-empty-result').length > 0) {
							return false;
						} else {
							const tr = $(e.target).closest('tr').find('.row-id').attr('id');
							if (!this.checkedRows.includes(tr)) {
								this.checkedRows = [tr];
							}
							let countActive = 0;
							let countActiveChecked = 0;
							let allDisabled = true;
							let allNotSystem = true;
							this.$refs["vuetable"].tableData.forEach(r => {
								if (this.checkedRows.includes(r.id) && r.isDeleted === false) {
									allDisabled = false;
									countActiveChecked++;
								}
								if (this.checkedRows.includes(r.id) && r.isSystem === true) {
									allNotSystem = false;
								}
								if (r.isDeleted === false) {
									countActive++;
								}
							});
							let atLeastOneActive = true;
							if (countActive != 0 && countActive - countActiveChecked === 0) {
								atLeastOneActive = false;
							}
							const callBackFunction = (key, _) => {
								this.emitAction(key, this.checkedRows, atLeastOneActive);
							}
							let items = null;
							if (this.checkedRows.length > 1) {
								if (allNotSystem) {
									if (allDisabled) {
										items = this.actionsMultipleDisabled
									} else {
										items = this.actionsMultiple
									}
								}
							} else {
								if ($(e.target).closest('tr').hasClass('disabled-row')) {
									items = this.actionsSingleDisabled;
								} else if ($(e.target).closest('tr').hasClass('system-row')) {
									items = this.actionsSingleSystem;
								} else {
									items = this.actionsSingle;
								}
							}
							return {
								callback: (key, options) => callBackFunction(key, options),
								items
							};
						}
					}
				});
			}
			$(`#${this.tableId}-selectAllCheckbox`).change(e => {
				this.selectedAll = e.target.checked;
				if (!e.target.checked) {
					this.checkedRows = [];
				}
			});
			$(`#${this.tableId} .datatables-dropdown .more-vertical`).click(e => {
				this.isVisibilityFieldsDropdownOpen = !this.isVisibilityFieldsDropdownOpen;
			});
			$(`#${this.tableId} .datatables-dropdown .dropdown-menu`).click(e => e.stopPropagation());
			$(`#${this.tableId} .datatables-dropdown .field-visibility`).change(e => {
				const fieldName = $(e.target).attr('id');
				const isVisible = e.target.checked;
				this.fields.forEach(field => {
					if (field.name == fieldName.replace('-visibility', '')) {
						field.visible = isVisible;
					}
				});
			});
			if (this.draggableRows) {
				$(`#${this.tableId} tbody`).sortable(this.draggableOptions);
				$(`#${this.tableId} tbody`).off('sortstop').on("sortstop", () => {
					this.updateStageOrder();
				});
			}
		},
		removeJqueryEventListeners() {
			$(`#${this.tableId}-selectAllCheckbox`).off('change');
			$(`#${this.tableId} .datatables-dropdown .dropdown-menu`).off('click');
			$(`#${this.tableId} .datatables-dropdown .field-visibility`).off('change');
			$(`#${this.tableId} .datatables-dropdown .more-vertical`).off('click');
			$(`#${this.tableId} th.sortable`).off('click');
			$.contextMenu('destroy', `#${this.tableId} .vuetable-body tr`);
		},
		makeQueryParams(sortOrder) {
			if (!this.hasCustomQUeryparams) {
				if (sortOrder.length > 0) {
					return {
						page: this.currentPage,
						pageSize: this.showPerPageInt,
						attribute: sortOrder[0].field,
						descending: sortOrder[0].direction == 'asc' ? 'false' : 'true'
					}
				} else {
					return {
						page: this.currentPage,
						pageSize: this.showPerPageInt,
					}
				}
			} else {
				if (sortOrder.length > 0) {
					this.customQueryparams.attribute = sortOrder[0].field;
					this.customQueryparams.descending = sortOrder[0].direction == 'asc' ? 'false' : 'true';
				}
				this.customQueryparams.page = this.currentPage;
				this.customQueryparams.pageSize = this.showPerPageInt;
				if (this.httpMethod.toLowerCase() == 'post') {
					const params = new URLSearchParams();
					for (let p in this.customQueryparams) {
						params.append(p, this.customQueryparams[p]);
					}
					return params;
				} else {
					return this.customQueryparams;
				}
			}
		},
		showMoreAction() {
			this.showPerPageInt = this.showPerPageInt + this.loadMoreCount;
			this.$refs["vuetable"].refresh();
		},
		loadSuccess(object) {
			if (this.objPath(object.data, this.dataPath)) {
				const totalShown = this.objPath(object.data, this.dataPath).length;
				const total = object.data.rowCount ? object.data.rowCount : object.data.result.rowCount;
				const remaining = total - totalShown;
				if (totalShown < total) {
					this.loadMoreCount = remaining >= 20 ? 20 : remaining;
				} else {
					this.loadMoreCount = false;
				}
			}
		},
		mosedownDrag(e) {
			$(e.target).addClass('dragging');
		},
		mouseUpDrag(e) {
			$(e.target).removeClass('dragging');
		},
		mouseLeaveDrag(e) {
			$(e.target).removeClass('dragging');
		},
		updateStageOrder() {
			let tableData = [];
			this.$refs["vuetable"].tableData.forEach(row => {
				tableData.push({
					stageId: row.id,
					order: row.order
				});
			});
			let newSortOrder = [];
			$.each($(`#${this.tableId} tbody`).children(), (index, row) => {
				newSortOrder.push({
					stageId: $(row).find('.drag-icon-element').attr('id'),
					order: index,
				});
			});
			this.$emit('dragResult', newSortOrder);
		},
		loadingStart() {
			this.dataLoaded = false;
			this.loaderHeight = this.tableHeight;
		},
		loadingStop() {
			this.dataLoaded = true;
			this.$emit('onLoad', this.$refs["vuetable"].tableData);
			this.tableHeight = $(`#${this.tableId}`).height() + 'px';
			if (this.customQueryparams.attribute !== 'isDeleted') {
				this.$refs["vuetable"].tableData = this.sortTableData(this.$refs["vuetable"].tableData);
			}
		},
		getRowCount() {
			return this.$refs["vuetable"].tableData.length;
		},
		objPath(obj, is, value) {
			if (typeof is == 'string')
				return this.objPath(obj, is.split('.'), value);
			else if (is.length == 1 && value !== undefined)
				return obj[is[0]] = value;
			else if (is.length == 0)
				return obj;
			else
				return this.objPath(obj[is[0]], is.slice(1), value);
		},
		sortTableData(array) {
			if (array) {
				return array.sort(function (x, y) {
					return (x.isDeleted === y.isDeleted) ? 0 : x.isDeleted ? 1 : -1;
				});
			} else {
				return [];
			}
		}
	},
	watch: {
		checkedRows: function (newVal) {
			this.selectedAll = newVal.length == this.$refs["vuetable"].countTableData;
		},
		selectedAll: function (newVal) {
			this.changeChecboxVal(newVal);
			if (this.$refs["vuetable"].tableData) {
				if (newVal) {
					this.checkedRows = [];
					this.$refs["vuetable"].tableData.map(e => this.checkedRows.push(e.id));
				}
			}
		},
		reload: function () {
			this.checkedRows = [];
		},
		showPerPage: function () {
			this.currentPage = 1;
			this.$refs["vuetable"].refresh();
		}
	}
});
