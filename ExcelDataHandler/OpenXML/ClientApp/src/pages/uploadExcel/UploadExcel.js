import React, { useState } from 'react';
import axios from 'axios';

function UploadExcel() {

    // 狀態變數，用於跟蹤所選擇的文件、是否有標頭以及API回應資料
    const [selectedFile, setSelectedFile] = useState(null);
    const [hasHeader, setHasHeader] = useState(false);
    const [responseData, setResponseData] = useState(null);

    // 處理文件選擇事件，更新selectedFile狀態
    const handleFileChange = (event) => {
        setSelectedFile(event.target.files[0]);
    };

    // 處理上傳按鈕點擊事件，上傳文件並處理API回應
    const handleUpload = async () => {
        if (!selectedFile) {
            alert('請選擇一個Excel文件');
            return;
        }

        const formData = new FormData();
        formData.append('file', selectedFile);

        try {
            // 使用axios發送POST請求，包括選擇的文件和是否有標頭的資料
            const response = await axios.post('/api/OpenXML/importExcel', formData, {
                headers: {
                    'Content-Type': 'multipart/form-data',
                },
                params: {
                    hasHeader: hasHeader,
                },
            });


            // 將API回應資料儲存在狀態變數中
            setResponseData(response.data);

            console.log('API成功：', response.data);
        } catch (error) {
            console.error('上傳失敗：', error);
        }
    };

    return (
        <div>

            <h2>上傳Excel文件</h2>

            <input type="file" accept=".xlsx,.xls" onChange={handleFileChange} />
            <label style={{ marginRight: '10px' }}>
                有標頭
                <input
                    type="checkbox"
                    checked={hasHeader}
                    onChange={(event) => setHasHeader(event.target.checked)}
                />
            </label>
            <button onClick={handleUpload}>上傳</button>

            {responseData && (
                <div>
                    <h3>API回應：</h3>
                    <pre>{JSON.stringify(responseData, null, 2)}</pre>
                </div>
            )}

        </div>
    );
}

export default UploadExcel;