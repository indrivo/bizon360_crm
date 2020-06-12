Vue.component('Checkbox', {
	template: `
		<div class="custom-control custom-checkbox">
			<input type="checkbox" class="custom-control-input" id="selectAll" v-model="selectedAll" v-on: change="selectAll()"/>
			<label class="custom-control-label" for="selectAll"></label>
		</div>`,
	data() {
		return {
			error: "",
			isValid: ""
		};
	},
	methods: {
		click() {
		}
	}
});