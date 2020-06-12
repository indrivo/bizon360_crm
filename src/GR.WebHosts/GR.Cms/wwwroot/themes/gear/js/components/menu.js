Vue.component('Menu', {
    template: `
		<nav class="menu main-menu mt-2">
			<ul class="first-level">
				<MenuLink
					v-for="menuLink in menuLinks"
					:key="menuLink.id"
					:object="menuLink"
					:openedTopParent="openedTopParent"
					@clicked="onLinkCLick"
					:currentPadding="10"
				/>
			</ul>
		</nav>`,
    data() {
        return {
            openedTopParent: null,
			menuLinks: [],
			breadcrumbs: []
        };
	},
	computed: {
		isSidebarOpen() { return this.$store.state.sidebarOpen }
	},
    beforeCreate: async function () {
        Promise.all([customAjaxRequest(apiEndpoints.GetMenus, 'GET'),
        customAjaxRequest(apiEndpoints.PipeLines.GetAll, 'GET')])
            .then(results => {
                const pipeLinesMenuId = "4549176b-9065-4a6e-be0c-e25176dfec4f";
                const menus = results[0];
				const pipeLines = this.sortPipeline(results[1], 'name');
                for (let i = 0; i < menus.length; i++) {
                    if (menus[i].id === pipeLinesMenuId) {
                        for (let j = 0; j < pipeLines.length; j++) {
                            menus[i].children.push({
                                order: j,
                                name: pipeLines[j].name,
                                children: [],
                                href: `/PipeLine/PipeLineLeads?id=${pipeLines[j].id}`
                            });
                        }
                    }
                }
                this.menuLinks = menus;
				this.$nextTick(() => this.attachEventListeners());
            }).catch(e => {
                toast.notifyErrorList(e);
				console.warn(e);
			}).finally(()=> {
				this.$store.dispatch("chanegMenuLoadedAction");
			});
	},
    methods: {
		onLinkCLick(value) {
			this.openedTopParent = value;
		},
		sortPipeline(array, prop) {
			return array.sort((a,b) => {
				const bandA = a[prop].toUpperCase();
				const bandB = b[prop].toUpperCase();
				let comparison = 0;
				if (bandA > bandB) {
					comparison = 1;
				} else if (bandA < bandB) {
					comparison = -1;
				}
				return comparison;
			});
		},
        attachEventListeners() {
            $(".sidebar-dropdown a").click(function () {
                $(this)
                    .toggleClass("open")
                    .siblings(".sidebar-submenu")
                    .slideToggle();
                $(this)
                    .closest(".sidebar-dropdown")
                    .toggleClass("open")
                    .siblings(".sidebar-dropdown")
                    .removeClass("open")
                    .find(".sidebar-submenu")
                    .slideUp();
                $(this)
                    .closest(".sidebar-dropdown")
                    .siblings(".sidebar-dropdown")
                    .find("a.dropdown-link")
                    .removeClass("open");
            });
            $(".sidebar-dropdown a.active")
                .parents(".sidebar-submenu")
                .addClass("open")
                .slideDown();

            $(".sidebar-dropdown a.active")
                .parents(".sidebar-submenu")
                .addClass("open")
                .siblings("a.dropdown-link")
                .addClass("open");

            $(".sidebar-dropdown a.active")
                .parents(".sidebar-dropdown")
                .addClass("open");
        }
    },
    beforeDestroy() {
        $(".sidebar-dropdown a").off("click");
	},
	watch: {
		isSidebarOpen(val) {
			if (!val) {
				const menu = $('.sidebar-dropdown.open');
				menu.removeClass("open");
				menu.find('.dropdown-link').removeClass("open").siblings(".sidebar-submenu").slideUp();
			} else {
				$(".sidebar-dropdown a.active")
					.parents(".sidebar-submenu")
					.addClass("open")
					.slideDown();

				$(".sidebar-dropdown a.active")
					.parents(".sidebar-submenu")
					.addClass("open")
					.siblings("a.dropdown-link")
					.addClass("open");

				$(".sidebar-dropdown a.active")
					.parents(".sidebar-dropdown")
					.addClass("open");
			}
		}
	}
});