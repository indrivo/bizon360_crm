Vue.component('UserBlock', {
	template: `
		<div class="d-flex cursor-pointer" @click.stop.prevent="toggleSidebar">
            <div class="d-md-block d-none mr-3">
                <h6 class="mb-0">{{ fullName }}</h6>
                <p class="font-size-12 mb-0">{{ user.jobPosition }}</p>
            </div>
            <ul class="list-unstyled d-flex m-0">
                <li class="nav-item m-0">
                    <button
						type="button"
						class="btn btn-outline-primary nav-user-button user-rectangle position-relative"
					>
                        <span>{{ initials }}</span>
						<span
							v-if="$store.state.userNotifications.count > 0"
							class="has-notifications"
						>
						</span>
                    </button>
                </li>
            </ul>
        </div>
	`,
	props: {
		user: {
			type: Object,
			required: true
		}
	},
	computed: {
		initials() {
			const firstNameChar = this.user.firstName.charAt(0);
			const lastNameChar = this.user.lastName.charAt(0);
			return `${firstNameChar} ${lastNameChar}`;
		},
		fullName() {
			return `${this.user.firstName} ${this.user.lastName}`;
		},
		hasImage() {
			return this.user.imageUrl.length > 0 ? true : false;
		}
	},
	methods: {
		toggleSidebar() {
			this.$store.dispatch("toggleUserSidebarAction");
		}
	}
});
