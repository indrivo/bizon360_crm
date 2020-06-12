Vue.component('Modal', {
	template: `<div class="modal fade" :id="modalProps.id" role="dialog">
		<div class="modal-dialog" :class="computedSize" role="document">
			<div class="modal-content">
				<div class="modal-header">
					<h6 class="modal-title mb-0" id="modal-label">{{ modalProps.label }}</h6>
					<div class="close-icon" data-dismiss="modal" aria-label="Close">
					</div>
				</div>
				<div class="modal-inner">
					<form :id="'form-' + modalProps.id" @submit.prevent="modalProps.onSubmit" :key="inputsKey">
						<div class="modal-body">
							<div class="form-inputs row custom-row">
								<component
									v-for="(option, index) in modalProps.formInputs"
									:key="option.props.id"
									v-bind:is="option.name"
									v-bind="option.props"
									@newValue="emitValue"
									:refresh="refreshInputs"
									@resetValue="emitResetValue"
								>
									<slot v-for="(_, name) in $slots" :name="name" :slot="name" />
									<template v-for="(_, name) in $scopedSlots" :slot="name" slot-scope="slotData">
										<div :class="'slot-' + name">
											<slot :name="name" v-bind="slotData" />
										</div>
									</template>
								</component>
							</div>
						</div>
						<div class="modal-footer">
							<component
								v-for="(option, index) in modalProps.formSubmits"
								:key="index"
								v-bind:is="option.name"
								v-bind="option.props"
								>
							</component>
						</div>
					</form>
				</div>
			</div>
		</div>
	</div>`,
	props: {
		modalProps: Object,
		refreshInputs: Number,
		inputsKey: Number
	},
	computed: {
		computedSize() {
			return this.modalProps.modalSize ? `modal-${this.modalProps.modalSize}` : '';
		}
	},
	mounted() {
		if (this.modalProps.onHide) {
			$(`#${this.modalProps.id}`).on('hide.bs.modal', (e) => {
				this.modalProps.onHide(e);
			});
		}
	},
	methods: {
		emitValue(val) {
			this.$emit('newValue', val);
		},
		emitResetValue(val) {
			this.$emit('resetValue', val);
		}
	}
})