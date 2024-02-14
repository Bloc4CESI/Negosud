import { ReactNode } from "react";

export interface ModalPropsInterface {
  title: string;
  message: string | ReactNode;
  type: "question" | "warning" | "danger";
  visible: boolean;
  onAccept: () => void;
  onCancel?: () => void;
}
