namespace Database.Models.Core;

public enum ActionResultType
{
    FailSave,
    FailEdit,
    FailDelete,
    FailGet,
    FailAdd,
    ObjectNotExist,
    NotValidData,
    ConflictData,
    SuccessSave,
    SuccessEdit,
    SuccessDelete,
    SuccessAdd,
    SuccessGet
}