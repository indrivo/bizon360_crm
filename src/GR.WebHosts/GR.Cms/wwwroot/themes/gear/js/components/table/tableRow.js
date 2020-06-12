Vue.component('TableRow', {
	template:`
	<tr :class="{selected: checked}" v-on:click="toggleCheckbox">
		<td>
			<div class="custom-control custom-checkbox">
				<input
					type="checkbox"
					class="custom-control-input datatables-bulk-check"
					:id="data.id"
					:data-parent-id="tableId"
					v-model="checked"
					v-on:click.stop.prevent
				/>
				<label
					class="custom-control-label"
					:for="data.id"
				></label>
			</div>
		</td>
		<TableCell
			v-for="(td, index) in columns"
			:key="td.key"
			:data="data[td.key]"
			:className="className"
		/>
		<td></td>
		</div>
	</tr>
	`,
	data() {
		return {
			checked: false
		}
	},
	props: {
		data: Array,
		className: String,
		tableId: String,
		columns: Array,
	},
	methods: {
		toggleCheckbox() {
			this.checked = !this.checked;
			this.$emit("checkboxVal", { id: this.data.id, value: this.checked });
		}
	}
});