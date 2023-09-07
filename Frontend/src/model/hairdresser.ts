export interface Hairdresser {
    id: number,
    firstName: string,
    lastName: string,
    mail: string,
    mobile: string
}

export interface State {
  hairdressers: Hairdresser[];
}
