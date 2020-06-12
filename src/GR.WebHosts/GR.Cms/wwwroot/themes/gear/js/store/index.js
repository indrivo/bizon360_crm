Vue.use(Vuex);

let storageStateSidebar = false;
if (typeof localStorage.sidebarOpen !== "undefined") {
    storageStateSidebar = JSON.parse(localStorage.sidebarOpen);
}

const state = {
	sidebarOpen: storageStateSidebar,
	userSidebarOpen: false,
    userNotifications: {
        count: 0,
        notifications: []
	},
	menuLoaded: false,
	customBreadcrumbs: []
};

const mutations = {
	toggleUserSidebar(state) {
		state.userSidebarOpen = !state.userSidebarOpen;
	},
	closeUserSidebar(state) {
		state.userSidebarOpen = false;
	},
    toggleSidebar(state) {
        state.sidebarOpen = !state.sidebarOpen;
        localStorage.sidebarOpen = state.sidebarOpen;
    },
    openSidebar(state) {
        state.sidebarOpen = true;
        localStorage.sidebarOpen = true;
    },
    changeCountNotifications(state, count) {
        state.userNotifications.count = count;
	},
	chanegMenuLoaded(state) {
		state.menuLoaded = true;
	},
	setCustomBreadcrumbs(state, list) {
		state.customBreadcrumbs = list;
	}
};
const actions = {
    toggleUserSidebarAction(context) {
        context.commit("toggleUserSidebar");
    },
    closeUserSidebarAction(context) {
        context.commit("closeUserSidebar");
    },
    toggleSidebarAction(context) {
        context.commit("toggleSidebar");
    },
    openSidebarAction(context) {
		context.commit("openSidebar");
    },
    changeCount(context, count) {
        context.commit("changeCountNotifications", count);
    },
	setCustomBreadcrumbsAction(context, list) {
		context.commit("setCustomBreadcrumbs", list);
    },
    chanegMenuLoadedAction(context) {
		context.commit("chanegMenuLoaded");
    }
};
const getters = {};

const store = new Vuex.Store({
    state,
    mutations,
    actions,
    getters
});
