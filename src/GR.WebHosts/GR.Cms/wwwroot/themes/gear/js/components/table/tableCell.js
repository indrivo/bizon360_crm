Vue.component('TableCell', {
	template: `<td :class="className">{{ dataValue }}</td>`,
	props: {
		data: [String, Array, Object, Boolean],
		className: String
	},
	computed: {
		dataValue() {
			if (typeof this.data == 'string' || typeof this.data == 'number') {
				return this.data;
			} else if (typeof this.data == 'boolean') {
				return this.data ? '✓' : '✘';
			} else if (typeof this.data == 'array') {
				let stringVal = '';
				this.data.map(elem => {
					stringVal = stringVal + `${elem.phone}\n`
				});
			} else {
				console.warn('cannot insert in table cell ' + this.data);
			}
		}
	}
});