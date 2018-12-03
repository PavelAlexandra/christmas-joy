export class User {
    id: number;
    email: string;
    userName: string;
    isAdmin: boolean;
    hashedPassword?: string
    age: number;
    secretSantaForId? : number;
    secretSantaFor: string;
}
