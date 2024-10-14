//#region Forms
export interface ILoginFormElements {
  formEmail: HTMLInputElement
  formPassword: HTMLInputElement
}

export interface IRegisterFormElements {
  formName: HTMLInputElement
  formEmail: HTMLInputElement
  formPassword: HTMLInputElement
  formConfirmPassword: HTMLInputElement
}

export interface ITransferFormElements {
  formRecipientId: HTMLInputElement
  formRecipientName: HTMLInputElement
  formAmount: HTMLInputElement
}

export interface IReplenishmentFormElements {
  formCardNumber: HTMLInputElement
  formCardDate: HTMLInputElement
  formCardCVV: HTMLInputElement
  formAmount: HTMLInputElement
}

export interface IAppealFormElements {
  formTitle: HTMLInputElement
  formMessage: HTMLInputElement
  formEmail: HTMLInputElement
}
//#endregion

//#region Props
export interface IContextProps {
  userBalance?: string
  setUserBalance?: (a: string) => void
  isAuthorized?: boolean
  setIsAuthorized?: (a: boolean) => void
  setIsLoading?: (a: boolean) => void
}

export interface ISearchUserElementsProps {
  className: string
  updateFormRecipientData: (a: string, b: string) => void
  handleNameOnInput: () => void
}

export interface IHistoryBlockProps {
  setIsShowHistoryBlock?: (a: boolean) => void
}

export interface IMessageBlockProps {
  isTransfer: boolean
  transactDateValue: string
  senderValue: string
  amountValue: string
  handleCloseMessageBlock: (e: any) => Promise<void>
}

export interface IHelpBlockProps {
  children: JSX.Element;
  question: string
}

export interface IBannerBlockProps {
  isTransfer: boolean
}
//#endregion

//#region Models
export interface ILoginData {
  Email: string
  Password: string
}

export interface IRegisterData extends ILoginData {
  Name: string
}

export interface IUserData {
  Id: string
  Name: string
  Email: string
  Balance: number
}

export interface ITransactionData {
  Id?: string
  CreatedDate?: Date
  SenderId?: string
  SenderName?: string
  RecipientId: string
  RecipientName: string
  Amount: number
  TransferCardNumber?: string
}

export interface IPresentTransactionData {
  Id?: string
  CreatedDateString: string
  CreatedDate: Date
  SenderId?: string
  SenderName?: string
  RecipientId: string
  RecipientName: string
  Amount: number
}

export interface IAppealData {
  Id?: string
  CreatedDate?: Date
  Title: string
  Message: string
  Email: string
}

export interface IAutoCompleteItem {
  id: string
  name: string
}
//#endregion
