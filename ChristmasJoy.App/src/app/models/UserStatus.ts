import { StatusPoints } from './StatusPoints';

export class UserStatus{
    userName: string;
    christmasStatus: string;
    points: number;
    noOfComments: number;
    id: number;

    public static nextStatusData(christmasStatus, points): any{
        let statusIndex = -1;
        let pointsMap = StatusPoints.map;
        for(var i=0; i< pointsMap.length; i++){
            if(pointsMap[i].Status == christmasStatus){
                statusIndex = i;
            }
        }
        if(statusIndex > -1 && statusIndex < pointsMap.length -1){
            let nextPoints = pointsMap[statusIndex +1].Points;
            return {
                Status: pointsMap[statusIndex +1].Status,
                Percentage: Math.round((points * 100)/nextPoints)
            };
        }
        return null;
    }
}
