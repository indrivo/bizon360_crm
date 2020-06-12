Vue.component('Select', {
	template: `
		<div :class="className">
			<div class="form-group">
				<label class="float-label" :for="id">{{ label }}<span class="required-input-label" v-if="required">*</span></label>
				<select
					class="selectpicker"
					:id="id"
					:disabled="disabled"
					v-model="newValue"
					:data-none-selected-text="noneSelectedText"
					:data-size="size"
					:data-live-search="searchableComputed"
					:multiple="multiple"
					:required="required"
					data-width="100%"
					data-virtual-scroll="1500"
					data-header="Select from list"
				>
					<option
						v-for="(option, index) in computedOptions"
						:key="index"
						:value="option.value"
						:data-content="option.dataContent"
						:selected="option.selected"
						:disabled="option.disabled"
					>{{ option.label }}</option>
				</select>
			</div>
		</div>
	`,
	props: {
		id: String,
		className: String,
		disabled: {
			type: Boolean,
			default: false
		},
		multiple: Boolean,
		label: String,
		options: {
			type: Array,
			default: () => []
		},
		required: {
			type: Boolean,
			default: false
		},
		value: {
			type: [String, Number, Array, Boolean],
			default: () => {return [] }
		},
		noneSelectedText: {
			type: String,
			default: 'Nothing selected'
		},
		size: { type: [Number, String], default: 'auto' },
		searchable: { type: Boolean, default: null }
	},
	mounted() {
		$(`#${this.id}`).selectpicker();
	},
	updated() {
		$(`#${this.id}`).selectpicker('refresh');
	},
	computed: {
		searchableComputed() {
			if (this.searchable === false) {
				return false;
			}
			return this.options.length > 10
		},
		newValue: {
			get() {
				return this.value;
			},
			set(val) {
				this.$emit("newValue", { value: val, id: this.id });
			}
		},
		computedOptions() {
			let newArray = [];
			this.options.forEach(option => {
				newArray.push({
					label: option.label,
					value: option.value,
					dataContent: option.dataContent,
					selected: option.value == this.value,
					disabled: option.disabled ? true : false
				});
			});
			let items = newArray.slice(0, 599);
			return items;
		}
	},
});