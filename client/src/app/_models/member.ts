import { Photo } from "./photo";


export interface Member {
    username: string;
    photoUrl: string;
    age: number;
    firstName: string;
    lastName: string;
    fatherName?: string;
    motherName?: string;
    knownAs: string;
    created: Date;
    lastActive: Date;
    gender: string;
    introduction: string;
    lookingFor: string;
    interests: string;
    country: string;
    photos: Photo[];
}