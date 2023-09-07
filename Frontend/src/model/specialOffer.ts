import {Service} from '@/model/service';

export interface SpecialOffer {
    id: number,
    code: string,
    serviceId: number,
    percentage: number,
    fromDate : string,
    toDate: string
}

export interface State {
    SpecialOffers: SpecialOffer[];
}
