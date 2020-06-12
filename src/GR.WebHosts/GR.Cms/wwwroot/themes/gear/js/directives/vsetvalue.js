Vue.directive('set-value', {
	bind(el, binding, vnode, oldVnode) {
		var model = vnode.data.directives.find(dir => dir.rawName === 'v-model');
		if (!model || !model.expression) {
			console.error('[vue-set-value warn] The element does not have v-model');
			return;
		}
		vnode.context[model.expression] = binding.value;
	}
})