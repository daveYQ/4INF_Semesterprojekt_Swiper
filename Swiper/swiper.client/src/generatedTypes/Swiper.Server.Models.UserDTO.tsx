
import { ImageDTO } from "Swiper.Server.Models.ImageDTO";

export class UserDTO
{
    id: string | null;
    userName: string | null;
    email: string | null;
    images: ImageDTO[] | null;
    age: number | null;
    residence: string | null;

    constructor()
    {
    
        this.id = null;
        this.userName = null;
        this.email = null;
        this.images = null;
        this.age = null;
        this.residence = null;
    }
}