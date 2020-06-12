//import Vue from "vue";
//import App from "./App.vue";
//import Vuex from "vuex";
//import store from './store'

//Vue.use(Vuex);
//Vue.config.productionTip = false;

//new Vue({
//    store,
//    render: h => h(App)
//}).$mount("#app");


//import Vue from "~/lib/vue/vue.esm.browser.js";



const MainTemplate = `
<div>
    main
</div>
`;

var app  = new Vue({
	el: "#app",
	//store,
	template: MainTemplate
});