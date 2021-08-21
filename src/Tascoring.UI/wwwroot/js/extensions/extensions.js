
function escapeRegExp(string) {
	return string.replace(/[.*+\-?^${}()|[\]\\]/g, '\\$&'); // $& means the whole matched string
}
function replaceAll(str, find, replace) {
	return str.replace(new RegExp(escapeRegExp(find), 'g'), replace);
}

const cleanUserName = function (username) {
	//"\n\t\t\t\t\t\n\t\t\t\t\t\t\n\t\t\t\t\t\n\t\t\t\t\tSee\n\t\t\t\t\t\n\t\t\t\t\t5\n\t\t\t\t"
	if (username.indexOf("\n") !== -1) {
		username = replaceAll(username, "\n", "");
	}
	if (username.indexOf("\t") !== -1) {
		username = replaceAll(username, "\t", "");
	}
	if (username.indexOf("\r") !== -1) {
		username = replaceAll(username, "\r", "");
	}
	if (username.indexOf("\t5") !== -1) {
		username = replaceAll(username, "\t5", "");
	}
	return username;
}