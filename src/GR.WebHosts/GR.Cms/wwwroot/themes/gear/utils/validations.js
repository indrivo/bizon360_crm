const fieldValidations = {
	name: {
		regex: /^[a-zA-Z0-9\s.,\ă\â\î\ț\ș\Â\Ă\Î\Ț\Ș]{2,128}$/,
		regexInput: /^[a-zA-Z0-9\s.,\ă\â\î\ț\ș\Â\Ă\Î\Ț\Ș]{0,128}$/,
		error: 'system_field_invalid_chars_or_length'
	},
	nameLetters: {
		regex: /^[a-zA-Z\s.,\ă\â\î\ț\ș\Â\Ă\Î\Ț\Ș]{2,50}$/,
		regexInput: /^[a-zA-Z\s.,\ă\â\î\ț\ș\Â\Ă\Î\Ț\Ș]{0,50}$/,
		error: 'system_field_invalid_chars_or_length'
	},
	varChar50: {
		regex: /^[a-zA-Z0-9\s.,\ă\â\î\ț\ș\Â\Ă\Î\Ț\Ș]{0,50}$/,
		regexInput: /^[a-zA-Z0-9\s.,\ă\â\î\ț\ș\Â\Ă\Î\Ț\Ș]{0,50}$/,
		error: 'system_field_invalid_chars_or_length'
	},
	varChar128: {
		regex: /^[\s\S]{2,128}$/,
		regexInput: /^[\s\S]{0,128}$/,
		error: 'system_field_invalid_chars_or_length'
	},
	varChar250: {
		regex: /^[a-zA-Z0-9\s().,\ă\â\î\ț\ș\Â\Ă\Î\Ț\Ș]{0,250}$/,
		regexInput: /[\s\S\ă\â\î\ț\ș\Â\Ă\Î\Ț\Ș]{0,250}$/,
		error: 'system_field_invalid_chars_or_length'
	},
	varChar500: {
		regex: /^[a-zA-Z0-9\s.,\ă\â\î\ț\ș\Â\Ă\Î\Ț\Ș]{0,5200}$/,
		regexInput: /^[a-zA-Z0-9\s\ă\â\î\ț\ș\Â\Ă\Î\Ț\Ș]{0,500}$/,
		error: 'system_field_invalid_chars_or_length'
	},
	email: {
		regex: /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/,
		error: 'system_field_invalid_email_address'
	},
	phone: {
		regex: /^[\d](?: ?\d){7,7}$/,
		regexInput: /^[\d](\d){0,7}$/,
		error: 'system_field_invalid_phone'
	},
	intNum: {
		regex: /^[0-9]{0,100}$/,
		regexInput: /^[0-9]{0,100}$/,
		error: 'system_field_invalid_field'
	},
	naturalNum: {
		regex: /^[1-9]{1}[0-9]{0,100}$/,
		regexInput: /^[1-9]{1}[0-9]{0,100}$/,
		error: 'system_field_invalid_field'
	},
	percentage: {
		regex: /(^100(\.0{1,2})?$)|(^([1-9]([0-9])?|0)(\.[0-9]{1,2})?$)/,
		regexInput: /^[0-9]{1}[0-9\.]{0,4}$/,
		error: 'system_field_invalid_field'
	},
	textField: {
		regex: /^.{0,200}$/,
		regexInput: /^.{0,50}$/,
		error: 'system_field_invalid_field'
	},
	zip: {
		regex: /^[0-9a-zA-Z\-]{0,25}$/,
		regexInput: /^[0-9a-zA-Z\-]{0,25}$/,
		error: 'system_field_invalid_field'
	},
	iban: {
		regex: /^[A-Z]{2}[0-9]{2}(?:[ ]?[0-9]{4}){4}(?:[ ]?[0-9]{1,2})?$/,
		regexInput: /^[0-9a-zA-Z\s]{0,128}$/,
		error: 'system_field_invalid_iban_code'
	},
	fiscalCodeMd: {
		regex: /^[0-9]{7,13}$/,
		regexInput: /^[0-9]{0,13}$/,
		error: 'system_field_invalid_fiscal_code'
	},
	password: {
		regex: /^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$/,
		error: 'system_field_invalid_password'
	}
}

function fieldValidationFunc(value, fieldName) {
	const vName = fieldValidations[fieldName];
	return value.match(vName.regex) ? true : window.translate(vName.error);
}

function fieldValidationInputFunc(value, fieldName) {
	if (value) {
		const vName = fieldValidations[fieldName];
		return value.match(vName.regexInput) ? true : false;
	} else {
		return ''; 
	}
}