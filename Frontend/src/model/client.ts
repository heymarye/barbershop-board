export interface Client {
    id: number,
    firstName: string,
    lastName: string,
    address: string,
    mail: string,
    mobile: string
}

export interface State {
    clients: Client[];
}
