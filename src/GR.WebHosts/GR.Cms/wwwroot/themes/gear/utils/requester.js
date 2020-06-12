const customAjaxRequest = async (requestUrl, requestType, requestData = {}) => {
	return new Promise((resolve, reject) => {
		$.ajax({
			url: requestUrl,
			type: requestType,
			data: requestData,
			success: data => {
				if (Array.isArray(data)) {
					resolve(data);
				}
				else {
					if (data.is_success) {
						resolve(data.result);
					} else if (!data.is_success) {
						reject(data.error_keys);
					} else {
						resolve(data);
					}
				}
			},
			error: e => {
				reject(e);
			}
		});
	});
}
