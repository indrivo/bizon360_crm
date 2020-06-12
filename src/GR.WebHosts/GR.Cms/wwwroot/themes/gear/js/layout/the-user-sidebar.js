Vue.component('TheUserSidebar', {
	template: `
		<div
			class="user-sidebar px-20"
			:class="{ open: isSidebarOpen }"
			v-click-outside="closeModal"
		>
			<a class="d-block mt-20px close-icon" @click.stop.prevent="closeModal"></a>
			<div class="current-date mt-20px">
				<p class="p-xs mb-0">{{ currentDate }}</p>
			</div>
			<div class="d-block mt-1 side-nav-links">
				<div class="side-nav-divider mb-2"></div>
				<a href="/account/account">My Account</a>
				<a href="/account/editAccount">Edit Account</a>
				<a href="/taskmanager">My Tasks</a>
				<div class="side-nav-divider my-2"></div>
				<button type="button" class="btn sa-logout">
					Log out
				</button>
			</div>
			<div class="position-relative" style="min-height: 200px">
				<div v-if="notificationsLoaded" id="notify" class="mt-4">
					<div class="d-flex">
						<h5>Notifications 
							<span class="badge badge-danger rounded top-webkit-inline-box d-inline notification-counter ml-1">{{ notifications.length }}</span>
						</h5>
						<a v-if="notifications.length > 0" class="ml-auto" href="#" @click.stop.prevent="clearAll">Clear all</a>
					</div>
					<div class="d-block side-nav-notifications w-100">
						<div class="side-nav-divider mb-1"></div>
						<div class="notification-container mt-1">
							<ul class="list-unstyled notification-list position-relative mb-0 collapsed">
								<li v-for="n in notifications" class="notification hover-invisible-toggle">
									<div class="d-flex w-100 position-relative">
										<p class="p-xs font-weight-600 color-black mb-0">{{ n.subject }}</p>
										<span class="notification-delete hover-invisible close-icon" @click.stop.prevent="deleteNotification(n.id)"></span>
										<span class="ml-auto hover-hide">
											{{ convertToRelativeTime(n.created) }}
										</span>
									</div>
									<div class="d-flex mt-1 w-100">
										<div class="badge badge-outline-primary user-rectangle notification-user-button">
											{{ userInitials() }}
										</div>
										<div class="d-block w-100 ml-2">
											<p class="p-sm color-black mb-0 notification-description">
												<span class="d-block">Notification</span>
												{{ n.content }}
											</p>
										</div>
									</div>
								</li >
								<hr class="my-2">
							</ul>
						</div>
					</div>
				</div>
				<div v-show="!notificationsLoaded" class="section-loader" style="background-color: #ffffff !important;"><Loader/></div>
			</div>
		</div>
	`,
	data() {
		return {
			currentDate: moment().format('dddd, MMMM D, YYYY'),
			user: {},
			notifications: [],
			notificationsLoaded: false,
		}
	},
	computed: {
		isSidebarOpen() {
			return this.$store.state.userSidebarOpen;
		}
	},
	created() {
		customAjaxRequest(apiEndpoints.Users.GetCurrentUserInfo).then(user => {
			this.user = user;
		});
	},
	mounted() {
		new ST().registerLocalLogout(".sa-logout");
	},
	methods: {
		async getUserNotifications() {
			return new Promise((resolve, reject) => {
				customAjaxRequest(apiEndpoints.Notifications.GetNotificationsByUserId, 'GET', { userId: this.user.id }).then(result => {
					this.notifications = result;
					this.notificationsLoaded = true;
					resolve(true);
				}).catch(e => {
					toast.notifyErrorList(e);
					reject(false);
				});
			});
		},
		async deleteNotification(notificationId) {
			this.notifications = this.notifications.filter(n => n.id != notificationId);
			customAjaxRequest(apiEndpoints.Notifications.PermanentlyDeleteNotification, 'DELETE', { notificationId }).catch(() => {
				this.getUserNotifications();
			});
		},
		clearAll() {
			this.notifications = [];
			customAjaxRequest(apiEndpoints.Notifications.ClearAllByUserId, 'POST', { userId: this.user.id }).catch(() => {
				this.getUserNotifications();
			});
		},
		closeModal() {
			this.$store.dispatch("closeUserSidebarAction");
		},
		convertToRelativeTime(date) {
			return moment(date, 'DD.MM.YYYY HH:mm:ss A').toNow(true) + ' ago';
		},
		userInitials() {
			return this.user.userLastName && this.user.userName ? `${this.user.userName.charAt(0)} ${this.user.userLastName.charAt(0)}` : 'UP';
		}
	},
	watch: {
		isSidebarOpen(newVal) {
			if (newVal === true && !this.notificationsLoaded) {
				this.getUserNotifications();
			}
		}
	}
});