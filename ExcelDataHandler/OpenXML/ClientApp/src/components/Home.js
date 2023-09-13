import React, { Component } from 'react';

export class Home extends Component {
    static displayName = Home.name;

    render() {
        return (
            <div>
                <h1>Excel 功能</h1>
                <h2>上傳</h2>
                <ul>
                    <li><a href='/upload/'>OpenXML - UploadExcel</a> </li>
                </ul>
            </div>
        );
    }
}