import { ss, su, si, sd, execute, executeScalar } from "@/lib/Test";
import { Invoice } from "@/lib/Model/Invoice";
import moment, { Moment } from "moment";

export class Booking {
  public static CancelBooking(bookId: string): any {
    Invoice.Delete(bookId);
    let qry: any = ss()
      .from("bookings")
      .field("RoomId")
      .where("Id = ?", bookId);
    let result: any = execute(qry);

    if (result.length > 0) {
      let first: any = result[0];
      this.UpdateRoomState(first.RoomId, 1);
    }

    let qry2: any = sd()
      .from("bookings")
      .where("Id = ?", bookId);
    let result2: any = executeScalar(qry2);
    return result2;
  }

  public static CheckinBooking(bookId: string): any {
    let today: Moment = moment();
    let qry: any = ss()
      .from("bookings")
      .field("RoomId")
      .where("Id = ?", bookId);
    let result: any = execute(qry);

    if (result.length > 0) {
      let first: any = result[0];
      this.UpdateRoomState(first.RoomId, 3);
    }

    let qry2: any = su()
      .table("bookings")
      .set("CheckinAt", today.format("YYYY-MM-DD HH:mm:ss"))
      .where("Id = ?", bookId);
    let result2: any = executeScalar(qry2);
    return result2;
  }

  public static Checkout(bookId: string): boolean {
    try {
      let today: string = moment().format("YYYY-MM-DD HH:mm:ss");
      let qry: any = su()
        .table("bookings")
        .set("CheckoutAt", today)
        .where("Id = ?", bookId);

      Invoice.CloseInvoice(bookId);
      executeScalar(qry);
      return true;
    } catch (e) {
      // need to pay invoice first
      return false;
    }
  }

  private static UpdateRoomState(roomId: number, state: number): any {
    let qry: any = su()
      .table("rooms")
      .set("RoomStateId", state)
      .where("Id = ?", roomId);
    executeScalar(qry);
  }
}
