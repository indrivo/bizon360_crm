Vue.component('TextBlock', {
	template: `
		<div class="w-100">
			{{ value }}
		</div>
	`,
	props: {
		value: String
	}
});
