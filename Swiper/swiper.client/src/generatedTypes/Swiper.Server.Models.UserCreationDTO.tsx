

export class UserCreationDTO
{
    userName: string;
    email: string;
    password: string;

    constructor()
    {
    
        this.userName = "";
        this.email = "";
        this.password = "";
    }
}