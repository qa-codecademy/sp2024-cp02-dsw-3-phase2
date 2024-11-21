document.getElementById('registerForm').addEventListener('submit', async (e) => {
    e.preventDefault();
    const formData = new FormData(e.target);
    const userData = Object.fromEntries(formData.entries());

    try {
        const response = await fetch('http://localhost:5095/api/User/register', {//tuka menuvate spored vasiot localhost
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(userData)
        });
        const result = await response.json();
        document.getElementById('message').innerText = result;
    } catch (error) {
        console.error('Error:', error);
    }
});

document.getElementById('loginForm').addEventListener('submit', async (e) => {
    e.preventDefault();
    const formData = new FormData(e.target);
    const loginData = Object.fromEntries(formData.entries());

    try {
        const response = await fetch('http://localhost:5095/api/User/login', {//isto i tuka
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(loginData)
        });
		debugger
        const result = await response.json();
        document.getElementById('message').innerText = result;
    } catch (error) {
        console.error('Error:', error);
    }
});
