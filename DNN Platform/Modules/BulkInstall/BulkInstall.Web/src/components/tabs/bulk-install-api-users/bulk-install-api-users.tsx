import { Component, Host, h } from '@stencil/core';
import { User } from './bulk-install-api-users.model';
import state from "../../../stores/store";

@Component({
  tag: 'bulk-install-api-users',
  styleUrl: 'bulk-install-api-users.scss',
  shadow: true,
})
export class BulkInstallApiUsers {

  private users: User[] = [];
  private newUser: User = {
    name: '',
    apiKey: '',
    encryptionKey: '',
    bypassIPWhitelist: false,
  }
  
  private newUserModal: HTMLDnnModalElement;

  private createUser(_newUser: User): (event: MouseEvent) => void {
    alert('Method not implemented.');
    return;
  }

  private deleteUser(_user: User): (event: MouseEvent) => void {
    alert('Method not implemented.');
    return;
  }

  render() {
    return (
      <Host>
        <div class="row">
          <div class="col">
            <div class="button-row">
              <dnn-button
                size="small"
                onClick={() => this.newUserModal.show()}
              >
                {state.resx.NewApiUser}
              </dnn-button>
            </div>
            <div class="panel">
              <div class="panel-heading">
                <h3 class="panel-title">{state.resx.ApiUsers}</h3>
              </div>
              <div class="panel-body">
                <table class="table">
                  <thead>
                    <tr>
                      <th>{state.resx.Name}</th>
                      <th>{state.resx.ApiKey}</th>
                      <th>{state.resx.EncryptionKey}</th>
                      <th>{state.resx.BypassIpAllowList}</th>
                      <th>{state.resx.Action}</th>
                    </tr>
                  </thead>
                  <tbody>
                    {this.users.map((user) => (
                      <tr>
                        <td>{user.name}</td>
                        <td>{user.apiKey}</td>
                        <td>{user.encryptionKey}</td>
                        <td>{String(user.bypassIPWhitelist)}</td>
                        <td>
                          <dnn-button
                            appearance="danger"
                            size="small"
                            onClick={() => this.deleteUser(user)}
                          >
                            {state.resx.Delete}
                          </dnn-button>
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            </div>
          </div>
        </div>
        <dnn-modal
          ref={(el) => this.newUserModal = el}
          backdropDismiss
        >
          <form
            class="create-user"
            onSubmit={(event) => {
              event.preventDefault();
              this.createUser(this.newUser);
            }}
          >
            <h4>{state.resx.NewApiUser}</h4>
            <dnn-input
              type="text"
              label={state.resx.ApiUserNameText}
              helpText={state.resx.ApiUserNameHelp}
              required
            />
            <label>
              <dnn-checkbox checked={this.newUser.bypassIPWhitelist ? 'checked' : 'unchecked'}></dnn-checkbox>
              {state.resx.BypassIpAllowList}
            </label>
            <dnn-button formButtonType="submit">{state.resx.Create}</dnn-button>
          </form>
        </dnn-modal>
      </Host>
    );
  }
}
