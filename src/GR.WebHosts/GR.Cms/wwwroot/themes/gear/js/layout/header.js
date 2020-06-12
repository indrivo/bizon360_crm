Vue.component('Header', {
    template: `
	<header>
        <nav class="navbar navbar-light bg-white border-bottom py-0 px-40 main-nav">
            <Hamburger />
            <a href="/" class="navbar-brand d-md-inline-block d-none mr-auto">
				<img class="img-fluid" :src="logoUrl" alt=""/>
			</a>
            <UserBlock :user="user" />
        </nav>
    </header>`,
    data() {
        return {
            user: {
                firstName: "",
                lastName: "",
                jobPosition: "",
                imageUrl: ""
			},
			logoUrl: '../themes/gear/img/logo.png'
        };
    },
    mounted() {
        this.$store.dispatch("changeCount", 2);
        this.setUserInfo();
    },
    methods: {
        toggleSidebar() {
            this.$store.dispatch("toggleSidebarAction");
        },
        search(value) {
            value.length > 2 ? (window.location.href = `/search=${value}`) : "";
        },
		setUserInfo() {
			customAjaxRequest(apiEndpoints.Users.GetCurrentUserInfo).then(user => {
				this.user.firstName = user.userFirstName;
				this.user.lastName = user.userLastName;
				if (user.jobPositionId) {
					customAjaxRequest(apiEndpoints.CrmVocabularies.GetJobPositionById, 'GET', { id: user.jobPositionId }).then(jobPosition => {
						this.user.jobPosition = jobPosition.name;
					});
				}
			});
		}
    }
});

