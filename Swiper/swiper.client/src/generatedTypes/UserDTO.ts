
import { ImageDTO } from "./ImageDTO";

export class UserDTO
{
    id: string | null;
    userName: string | null;
    email: string | null;
    images: ImageDTO[] | null;
    age: number | null;
    residence: string | null;

    constructor(id?: string | null,userName?: string | null,email?: string | null,images?: ImageDTO[] | null,age?: number | null,residence?: string | null,)
    {
    
        this.id = id ?? null;
        this.userName = userName ?? null;
        this.email = email ?? null;
        this.images = images ?? null;
        this.age = age ?? null;
        this.residence = residence ?? null;
    }
}