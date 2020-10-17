import { Injectable } from "@angular/core";
import { HttpInterceptor, HttpRequest, HttpHandler } from "@angular/common/http";
import { UserService } from "../services/user.service";
import { Observable } from "rxjs";
import { } from "@angular/common/http/http";
import { GlobalsService } from "../services/globals.service";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private globalsService: GlobalsService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler) {
    let authToken = "";
    if (this.globalsService.session != null)
      authToken = this.globalsService.session.token;
    req = req.clone({
      setHeaders: {
        Authorization: "Bearer " + authToken
      }
    });
    return next.handle(req);
  }
}
