import { UserCreationDTO } from './Swiper.Server.Models.UserCreationDTO';
module App { 

    export class UserController {

        constructor(private $http: ng.IHttpService) { 
        } 
        
            public health = () : ng.IHttpPromise<IActionResult> => {
                
                return this.$http<IActionResult>({
                    url: `Health`, 
                    method: "get", 
                    data: null
                });
            };    
            public index = () : ng.IHttpPromise<IActionResult> => {
                
                return this.$http<IActionResult>({
                    url: `User`, 
                    method: "get", 
                    data: null
                });
            };    
            public getUserById = (id: string) : ng.IHttpPromise<IActionResult> => {
                
                return this.$http<IActionResult>({
                    url: `User/${id}`, 
                    method: "get", 
                    data: null
                });
            };    
            public delete = (id: string) : ng.IHttpPromise<IActionResult> => {
                
                return this.$http<IActionResult>({
                    url: `User/${id}`, 
                    method: "delete", 
                    data: null
                });
            };    
            public deleteAll = () : ng.IHttpPromise<IActionResult> => {
                
                return this.$http<IActionResult>({
                    url: `User`, 
                    method: "delete", 
                    data: null
                });
            };    
            public register = (userCreationDto: UserCreationDTO) : ng.IHttpPromise<IActionResult> => {
                
                return this.$http<IActionResult>({
                    url: `User/Register`, 
                    method: "post", 
                    data: userCreationDto
                });
            };    
            public logIn = (id: string, password: string, rememberMe: boolean) : ng.IHttpPromise<IActionResult> => {
                
                return this.$http<IActionResult>({
                    url: `User/LogIn/${id}?password=${encodeURIComponent(password)}&rememberMe=${rememberMe}`, 
                    method: "post", 
                    data: null
                });
            };    
            public logOff = () : ng.IHttpPromise<IActionResult> => {
                
                return this.$http<IActionResult>({
                    url: `User/LogOff`, 
                    method: "post", 
                    data: null
                });
            };    
            public isLoggedIn = () : ng.IHttpPromise<IActionResult> => {
                
                return this.$http<IActionResult>({
                    url: `User/ID`, 
                    method: "get", 
                    data: null
                });
            };    
            public like = (id: string) : ng.IHttpPromise<IActionResult> => {
                
                return this.$http<IActionResult>({
                    url: `User/Like?id=${encodeURIComponent(id)}`, 
                    method: "post", 
                    data: null
                });
            };    
            public getMatches = () : ng.IHttpPromise<IActionResult> => {
                
                return this.$http<IActionResult>({
                    url: `User/Matches`, 
                    method: "get", 
                    data: null
                });
            };    
            public uploadPfp = (file: IFormFile) : ng.IHttpPromise<IActionResult> => {
                
                return this.$http<IActionResult>({
                    url: `User/ProfilePicture`, 
                    method: "post", 
                    data: file
                });
            };
    }
    
    angular.module("App").service("UserService", ["$http", UserController]);
}
