﻿namespace ResourceLib.Method
{
    /// <summary>
    ///     字符枚举
    /// </summary>
    public enum TextType
    {
        CollectionIndexTitle,
        CollectionIndexTabCurrent,
        CollectionIndexTabCurrentDel,
        CollectionIndexTabManager,

        DosCommandTabDeploy,
        DosCommandTabDeployAuthentication,
        DosCommandTabDeployMasterAddress,
        DosCommandTabDeployPort,
        DosCommandTabDeployMaster,
        DosCommandTabDeploySlave,
        DosCommandTabDeploySlaveSource,
        DosCommandTabDeployDbPath,
        DosCommandTabDeployLogPath,
        DosCommandTabDeployLog,
        DosCommandTabDeployAppendMode,
        DosCommandTabBackup,
        DosCommandTabBackupServer,
        DosCommandTabBackupPort,
        DosCommandTabBackupDbName,
        DosCommandTabBackupDcName,
        DosCommandTabBackupPath,
        DosCommandTabExIn,
        DosCommandTabExInServer,
        DosCommandTabExInPort,
        DosCommandTabExInDbName,
        DosCommandTabExInDcName,
        DosCommandTabExInColumnList,
        DosCommandTabExInWorkfile,
        DosCommandTabExInImport,
        DosCommandTabExInExport,
        DosCommandTabExInOperation,
        DosCommandRun,
        DosCommandBrowse,
        DosCommandClean,
        DosCommandLogLevel,

        ServiceStatusTitle,
        ServiceStatusServerInfo,
        ServiceStatusDataBaseInfo,
        ServiceStatusCollectionInfo,
        ServiceStatusCurrentOperationInfo,

        //Controller
        CtlIndexCreateIndex,
        CtlIndexCreateDescription,
        CtlFieldInfoShow,

        //GFS
        GfsFilename,
        GfsLength,
        GfsChunkSize,
        GfsUploadDate,
        GfsMd5,
        GfsInsertOptionRemoteFileName,
        GfsInsertOptionOnlyFilename,
        GfsInsertOptionFullPath,
        GfsInsertOptionFileAlreadyExist,
        GfsInsertOptionJustAddIt,
        GfsInsertOptionRename,
        GfsInsertOptionSkipIt,
        GfsInsertOptionOverwrite,
        GfsInsertOptionStop,
        GfsInsertOptionIngoreSubFolder,
        GfsInsertOptionDirectorySeparatorChar,

        //DataBase_Status
        DataBaseStatusDataBaseName,
        DataBaseStatusCollectionCount,
        DataBaseStatusDataSize,
        DataBaseStatusFileSize,
        DataBaseStatusIndexCount,
        DataBaseStatusIndexSize,
        DataBaseStatusObjectCount,
        DataBaseStatusStorageSize,

        //Collection_Status
        CollectionStatusCollectionName,
        CollectionStatusObjectCount,
        CollectionStatusDataSize,
        CollectionStatusLastExtentSize,
        CollectionStatusStorageSize,
        CollectionStatusTotalIndexSize,
        CollectionStatusAverageObjectSize,
        CollectionStatusPaddingFactor,
        CollectionStatusMaxDocuments,
        CollectionStatusMaxSize,
        CollectionStatusIsCapped,
        CollectionStatusIsAutoIndexId,

        CollectionResumeAutoRefresh,
        CollectionStopAutoRefresh,

        IndexAsce,
        IndexDesc,
        IndexNoSort,
        IndexBackground,
        IndexRepeatDel,
        IndexSparse,
        IndexUnify,
        IndexName,
        IndexKeys,
        IndexVersion,
        IndexExpireData,
        IndexNameSpace,

        //Exception
        ExceptionNotConnected,
        ExceptionNotConnectedNote,
        ExceptionAuthenticationException,
        ExceptionAuthenticationExceptionNote,

        SelectedServer,
        SelectedDataBase,
        SelectedCollection,
        SelectedData,
        SelectedIndexes,
        SelectedIndex,
        SelectedGfs,
        SelectedUserList,

        //MessageBox
        CreateNewDataBase,
        CreateNewDataBaseInput,
        DropDataBase,
        DropDataBaseConfirm,
        CreateNewCollection,
        RenameCollection,
        RenameCollectionInput,
        DropCollection,
        DropCollectionConfirm,
        DropUser,
        DropUserConfirm,
        DropData,
        DropDataConfirm,
        RestoreConnectionConfirm,
        CopyIndex,
        MainStatusBarTextReady,
    }
}