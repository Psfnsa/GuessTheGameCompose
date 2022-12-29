<template>
    <div class="login">
        <h1>{{message}}</h1>
        <h1 v-if="isLogged">
            <button v-on:click="isRegisterForm = false, isLoginForm = true" :disabled='isLoginForm' :hidden='isHidden'>LoginForm </button>
            <button v-on:click="isRegisterForm = true, isLoginForm = false" :disabled='isRegisterForm' :hidden='isHidden'>RegisterForm</button>
        </h1>

        <fieldset v-if="isLogged && isLoginForm">
            <legend>Login details</legend>
            <input v-model="username" placeholder="Username" /><br />
            <input v-model="password" placeholder="Password" type="password" v-on:keyup.enter="retrieveCredentials(username, password)" /><br />
            <button v-on:click="retrieveCredentials(username, password)">Login</button>
        </fieldset>

        <fieldset v-if="isRegisterForm && isLogged">
            <legend>Register details</legend>
            <input v-model="username" placeholder="Username" /><br />
            <input v-model="password" placeholder="Password" type="password" v-on:keyup.enter="registerCredentials(username, password)" /><br />
            <button v-on:click="registerCredentials(username, password)">Register</button>
        </fieldset>

        <fieldset v-if="!isLogged">
            <h1>User {{username}}</h1>
            <button v-on:click="logoutFunction(), isHidden = false">Log out</button>
        </fieldset>

    </div>
</template>

<style>
    fieldset {
        font-size: 20px;
        padding: 15px;
        min-width: 1024px;
        text-align: center;
    }
</style>

<script>
    import axios from 'axios';

    const config = {
        headers: {
            Authorization: 'Bearer ' + localStorage.getItem('token'),
        },
    };

    export default {
        data() {
            return {
                credentials: [],
                username: "",
                password: "",
                id: "",
                message: "",
                isLogged: false,
                isRegister: false,
                isLoginForm: true,
                isRegisterForm: false,
                isHidden: false,
                responseTemp: [],
            };
        },

        async mounted() {
            this.init();
        },

        methods: {
            init() {
                if (localStorage.getItem('idUser') == null) {
                    localStorage.setItem('idUser', 0);
                }
                if (localStorage.getItem('idUser') == 0) {
                    this.isLogged = true;
                }

                if (localStorage.getItem('idUser') != 0) {
                    this.isLogged = false;
                }

                if (!this.isLogged) {
                    this.username = localStorage.getItem('username');
                }
                this.isRegister = false;
            },

            retrieveCredentials: function (username, password) {
                if (username && password && localStorage.getItem('idUser') == 0) {
                    this.username = username;
                    this.created();
                }
            },

            registerCredentials: function (username, password) {
                this.password = password;
                this.username = username;
                this.registeruser();
            },

            logoutFunction() {
                localStorage.setItem('idUser', 0);
                this.message = "You are logged out."
                this.password = "";
                this.username = "";
                localStorage.setItem('idUser', 0);
                localStorage.setItem('username', 0);
                this.init();
            },

            saveInLocalStorageFunction(response) {
                localStorage.setItem('idUser', response.data.value.id);
                localStorage.setItem('username', response.data.value.username);
                localStorage.setItem('token', response.data.value.token);
                this.isHidden = true;
                this.isLogged = false;
                this.message = "You are logged in.";
            },

            async created() {
                await axios.get('http://localhost:7279/api/GuessTheGame/login?username=' + this.username + "&password=" + this.password, null, config)
                    .then(response => {
                        this.credentials = response.data.value;
                        if (this.credentials.token != 0) {
                            this.id = this.credentials.id;
                            localStorage.setItem('idUser', this.id);
                            localStorage.setItem('username', this.username);
                            this.isLogged = false;
                            this.isRegister = true;
                            this.isHidden = true;
                            this.message = "You are logged in.";
                            localStorage.setItem('token', this.credentials.token);
                            //console.log("this.credentials.token " + this.credentials.token);
                        }
                        if (this.credentials.token == 0) {
                                this.message = "Username or Password is wrong.";
                                this.isRegister = true;
                            }
                        if (!this.isRegister) {
                            this.message = "User is not in DataBase, you can register this user";
                        }
                    });
            },

            async registeruser() {
                await axios.post('http://localhost:7279/api/GuessTheGame/register', {
                    "username": this.username,
                    "password": this.password,
                }, config)
                    .then(response => {
                        //console.log("response.data.value " + response.data.value);
                        if (response.data.value.id == 0) {
                            this.message = "User is already in DataBase";
                        }
                        if (response.data.value.id != 0) {
                            this.saveInLocalStorageFunction(response);
                            this.registercurrentlevel();
                        }
                    });
            },

            async registercurrentlevel() {
                await axios.post('http://localhost:7279/api/GuessTheGame/currentlevel?idUser=' + localStorage.getItem('idUser'), null, config);
            },
        },
    }
</script>
