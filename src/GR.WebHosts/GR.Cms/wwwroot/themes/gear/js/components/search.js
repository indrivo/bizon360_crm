Vue.component('Search', {
	template: `
		<div class="d-sm-flex d-none ml-auto">
			<input
				v-model="searchValue"
				type="text"
				class="form-control"
				:class="className"
				:id="id"
				:placeholder="placeholder"
				@keydown.enter="startSearch"
			/>
		</div>
	`,
	data() {
		return {
			searchValue: ""
		};
	},
	props: {
		id: String,
		className: String,
		placeholder: String,
		validator: Function
	},
	methods: {
		startSearch() {
			this.$emit("searchValue", this.searchValue);
		}
	}
});
