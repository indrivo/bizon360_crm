Vue.component('File', {
	template: `<div :class="className">
		<div class="form-group custom-file">
			<label
				:class="classNames"
				>{{ label }}<span class="required-input-label" v-if="required">*</span></label
			>
			<input
				type="file"
				class="form-control"
				:id="id"
				:placeholder="placeholder"
				:required="required"
				:disabled="disabled"
				@change="processFile($event)"
				:accept="acceptedExtensions"
			/>
			<label class="custom-file-label" :for="id">{{ fileName }}</label>
		</div>
    </div>`,
	data() {
		return {
			data: null
		};
	},
	props: {
		id: { type: String, required: true },
		className: String,
		name: String,
		label: String,
		disabled: Boolean,
		required: { type: Boolean, default: false },
		placeholder: String,
		acceptedExtensions: { type: String, default: '' }
	},
	computed: {
		classNames() {
			const c = this.disabled ? 'disabled ' : '';
			return c + ' float-label';
		},
		fileName() {
			const file = this.data;
			return file ? file.name : 'Choose file';
		}
	},
	methods: {
		processFile(event) {
			this.data = event.target.files[0];
			this.$emit('newValue', this.data);
		}
	}
});