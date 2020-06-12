Vue.directive('click-outside', {
	bind: function (el, binding, vnode) {
		el.eventSetDrag = function () {
			el.setAttribute('data-dragging', 'yes');
		}
		el.eventClearDrag = function () {
			el.removeAttribute('data-dragging');
		}
		el.eventOnClick = function (event) {
			var dragging = el.getAttribute('data-dragging');
			if (!(el == event.target || el.contains(event.target)) && !dragging) {
				vnode.context[binding.expression](event);
			}
		};
		document.addEventListener('touchstart', el.eventClearDrag);
		document.addEventListener('touchmove', el.eventSetDrag);
		document.addEventListener('click', el.eventOnClick);
		document.addEventListener('touchend', el.eventOnClick);
	}, unbind: function (el) {
		document.removeEventListener('touchstart', el.eventClearDrag);
		document.removeEventListener('touchmove', el.eventSetDrag);
		document.removeEventListener('click', el.eventOnClick);
		document.removeEventListener('touchend', el.eventOnClick);
		el.removeAttribute('data-dragging');
	},
});