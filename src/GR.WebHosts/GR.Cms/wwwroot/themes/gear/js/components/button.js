Vue.component('Button', {
	template: `
		<button
			:type="btnDOMType"
			:class="['btn', 'btn-' + btnType, buttonSize, className, waitingClass]"
			@click="onClickThis"
			@mousedown="onMouseDown"
			v-html="label"
			:disabled="disabled"
		>
		</button>`,
	props: {
		label: String,
		className: String,
		onClick: {
			type: Function,
			default: () => { }
		},
		onMouseDown: {
			type: Function,
			default: () => { }
		},
		disabled: Boolean,
		waiting: Boolean,
		btnDOMType: {
			type: String,
			default: 'button',
			validator: function (value) {
				return [
					'submit',
					'button',
					'reset'
				].indexOf(value) !== -1
			}
		},
		btnType: {
			type: String,
			validator: function (value) {
				return [
					'success',
					'warning',
					'danger',
					'info',
					'primary',
					'purple',
					'dark',
					'secondary',
					'light',
					'outline-success',
					'outline-warning',
					'outline-danger',
					'outline-info',
					'outline-primary',
					'outline-purple',
					'outline-dark',
					'outline-secondary',
					'outline-light'
				].indexOf(value) !== -1
			},
			default: 'info'
		},
		btnSize: {
			type: String,
			validator: function (value) {
				return ['lg','md','sm', ''].indexOf(value) !== -1
			},
			default: ''
		},
	},
	computed: {
		buttonSize() {
			return this.btnSize !== '' ? `btn-${this.btnSize}` : '';
		},
		waitingClass() {
			return this.waiting ? 'waiting' : '';
		}
	},
	methods: {
		onClickThis(e) {
			if (this.disabled || this.waiting) {
				e.preventDefault();
				e.stopPropagation();
			} else {
				this.onClick(e);
			}
		}
	}
});