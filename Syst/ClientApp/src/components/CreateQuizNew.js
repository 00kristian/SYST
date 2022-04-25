export default CreateQuizNew

function CreateQuizNew(props) {
    return (
        <div>
            {contents}
            <button className="btn btn-primary" type="button" onClick={() => this.addQuestion()}>+</button> 
            <button className="btn btn-minus_quiz" type="button" onClick={() => this.removeQuestion()}>-</button>        
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <button className="btn btn-primary btn-right" onClick={this.rerouteToConfirmation}>Confirm</button>
            <button className="btn btn-primary" onClick={this.deleteQuiz}>Back</button>
        </div>
    );
}