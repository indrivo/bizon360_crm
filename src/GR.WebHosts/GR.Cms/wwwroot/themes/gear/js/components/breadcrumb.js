Vue.component('Breadcrumb', {
	template: `
		<div :class="className">
			<span v-for="(b, i) in breadcrumbs" :key="i">
				<span v-if="i !=0 ">/</span>
				<span v-html="b"></span>
				&nbsp;
			</span>
		</div>
	`,
	props: {
		className: String
	},
	data() {
		return {
			breadcrumbs: [],
		}
	},
	computed: {
		customBreadcrumbs() {
			return this.$store.state.customBreadcrumbs;
		}
	},
	created() {
		if (this.customBreadcrumbs.length > 0) {
			this.breadcrumbs = this.customBreadcrumbs;
		} else {
			this.breadcrumbs = [];
			const menuTree = $('.menu-link a.active');
			if (menuTree.length > 0) {
				let run = true;
				let tree = menuTree;
				do {
					tree = tree.parent().closest('.sidebar-dropdown');
					if (tree.length > 0) {
						const text = tree.children('.dropdown-link')
							.find('.menu-link-label')
							.clone()
							.children()
							.remove()
							.end()
							.text();
						this.breadcrumbs.unshift($.trim(text));
					} else {
						run = false;
					}
				} while (run);
			}
			if (this.breadcrumbs.length > 0) {
				this.breadcrumbs.push($.trim(menuTree.text()));
			}
		}
	}
});