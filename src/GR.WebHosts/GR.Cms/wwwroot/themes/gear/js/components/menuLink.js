Vue.component('MenuLink', {
	template: `
    <li class="menu-link" :class="{ 'sidebar-dropdown': haveChildrens }">
        <a
            class="dropdown-link w-100 d-flex align-items-center"
            :class="{ active: isCurrentUrl() }"
            v-if="haveChildrens"
            v-on:click.stop.prevent="openSidebar"
            href="#"
            :style="{ paddingLeft: padding + 'px' }"
        >
            <span
                v-if="haveIcon"
                class="menu-link-icon"
            ><i :data-feather="object.icon"></i></span>
            <span v-else class="menu-link-icon menu-link-default-icon"></span>
            <div class="d-flex w-100 menu-link-label" :class="menuLinkClass">
                {{ object.name }}
                <span class="dropdown-icon ml-auto"></span>
            </div>
        </a>
        <a
            :style="{ paddingLeft: padding + 'px' }"
            class="w-100 d-flex align-items-center"
            :class="{ active: isCurrentUrl() }"
            :href="objHref"
            v-else
			v-on:click="openSidebar"
        >
           <span
                v-if="haveIcon"
                class="menu-link-icon"
            ><i :data-feather="object.icon"></i></span>
            <span v-else class="menu-link-icon menu-link-default-icon"></span>
            <span class="menu-link-label" :class="menuLinkClass">{{
                object.name
            }}</span>
        </a>
        <div v-if="haveChildrens" class="sidebar-submenu collapse">
            <ul class="next-level">
                <MenuLink
                    v-for="children in object.children"
                    :key="children.id"
                    :object="children"
                    :currentPadding="padding"
                />
            </ul>
        </div>
    </li>
`,
	props: {
		openedTopParent: String,
		object: Object,
		currentPadding: Number
	},
	data() {
		return {
			isOpen: false,
		};
	},
	computed: {
		isSidebarOpen() { return this.$store.state.sidebarOpen },
		menuLinkClass() {
			return this.isSidebarOpen ? "open" : "closed";
		},
		objHref() {
			if (this.object.href) {
				return this.object.href;
			} else {
				return "#";
			}
		},
		padding() {
			return this.$store.state.sidebarOpen
				? this.currentPadding + 10
				: this.currentPadding;
		},
		haveIcon() {
			if (
				this.object.icon &&
				this.object.icon.length > 0 &&
				this.object.icon != "none"
			) {
				return true;
			} else {
				return false;
			}
		},
		haveChildrens() {
			if (this.object.children.length > 0) {
				return true;
			} else {
				return false;
			}
		}
	},
	mounted() {
		feather.replace({
			width: 17,
			height: 17,
		});
	},
	updated() {
		feather.replace({
			width: 17,
			height: 17,
		});
	},
	methods: {
		openSidebar() {
			if (!this.isSidebarOpen) {
				this.$store.dispatch("openSidebarAction");
			}
		},
		isSame(val1, val2) {
			return val1 === val2 ? true : false;
		},
		findOpenParent() {
			let openParent = "";

			const func = obj => {
				if (!obj.parentMenuItemId) {
					return;
				}
				openParent = obj.parentMenuItemId;
				func(obj.parentMenuItem);
			};

			func(this.object);

			return openParent;
		},
		isCurrentUrl() {
			const currentUrl = document.location.pathname + document.location.search;
			return this.object.href === currentUrl ? true : false;
		}
	}
});