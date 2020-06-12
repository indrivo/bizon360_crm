Vue.component('Textarea', {
	template: `
		<div :class="className">
			<div class="form-group">
				<label :for="id">{{ label }}<span class="required-input-label" v-if="required">*</span></label>
				<customCkeditor id="ckeditor1" name="editor1" v-model="newValue" :config="editorConfig"></customCkeditor>
			</div>
		</div>
	`,
	data() {
		return {
			error: "",
			isValid: "",
			editorConfig: {
				removeButtons: 'save-to-pdf',
				availableTokens: [
					["Current Date", "CurrentDate"],
					["Organization Name", "OrganizationName"],
					["IDNO", "IDNO"],
					["IBAN", "IBAN"],
					["Bank", "Bank"],
					["Phone", "Phone"],
					["Email", "Email"],
					["Cod Tva", "CodTva"],
					["Cod Swift", "CodSwift"],
					["Region", "Region"],
					["City", "City"],
					["Street", "Street"],
					["Contact", "Contact"],
					["Currency", "Currency"],
					["Value", "Value"],
					["Commission", "Commission"]
				],
				tokenStart: '{',
				tokenend: '}',
				language: 'en'
			},
		};
	},
	props: {
		id: String,
		className: String,
		name: String,
		label: String,
		disabled: Boolean,
		required: Boolean,
		placeholder: String,
		value: String
	},
	computed: {
		newValue: {
			get() {
				return this.value;
			},
			set(val) {
				this.$emit("newValue", { value: val, id: this.id });
			}
		}
	}
});