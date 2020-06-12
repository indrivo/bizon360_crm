Vue.component('Input', {
	template: `
		<th v-if="isHeader">
			<div slot="id" slot-scope="props" class="custom-control custom-checkbox">
				<input type="checkbox" class="custom-control-input" :id="selectAllCheck"/>
				<label class="custom-control-label" :for="selectAllCheck"></label>
			</div>
		</th>
		<td v-else>
			<div slot="id" slot-scope="props" class="custom-control custom-checkbox">
				<input type="checkbox" class="custom-control-input" :id="selectAllCheck"/>
				<label class="custom-control-label" :for="selectAllCheck"></label>
			</div>
		</td>
	`,
	mixins: [Vuetable.VuetableFieldMixin],
	methods: {
		
	}
});