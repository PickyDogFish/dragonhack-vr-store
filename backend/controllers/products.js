const listAllCategories = (req, res) => {
    res.status(200).json({status: "yes"});
};

module.exports = {
    listAllCategories
}