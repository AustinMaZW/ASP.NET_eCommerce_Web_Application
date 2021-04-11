window.onload = function ()
{
	let errDiv = document.getElementById("err_msg");

	let loginForm = document.getElementById("loginform");
	loginForm.onsubmit = function () {
		let elemUname = document.getElementById("username");
		let elemPwd = document.getElementById("password");

		let username = elemUname.value.trim();
		let password = elemPwd.value.trim();

		if (username.length === 0 || password.length === 0) {
			errDiv.innerHTML = "Please fill up all fields.";
			return false;	// cancel form submission
		}

		return true;	// allow form submission to continue
	}
}