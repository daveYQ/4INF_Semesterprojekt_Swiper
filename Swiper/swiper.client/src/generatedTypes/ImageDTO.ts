

export class ImageDTO
{
    id: number | null;
    data: number[] | null;

    constructor(id?: number | null,data?: number[] | null,)
    {
    
        this.id = id ?? null;
        this.data = data ?? null;
    }
}