Vue.component('Hamburger', {
	template: `
		<button
			type="button"
			class="hamburger hamburger--collapse"
			:class="componentClass()"
			v-on:click="toggleSidebar()"
		>
			<span class="hamburger-box">
				<span class="hamburger-inner"></span>
			</span>
		</button>
	`,
	props: {
		className: String
	},
	methods: {
		componentClass() {
			return this.$store.state.sidebarOpen
				? `${this.className} is-active`
				: this.className;
		},
		toggleSidebar() {
			this.$store.dispatch("toggleSidebarAction");
		}
	}
});
